using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Application.Models.Users;
using FoodCollectionsBackend.Domain.Entities;
using MediatR;
using FoodCollectionsBackend.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FoodCollectionsBackend.Infrastructure.Services;

public class AuthCommandService(IAppDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService)
    : IAuthCommandService
{
    public async Task<Result<LoginResponse>> LoginAsync(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var usernameOrEmail = request.UsernameOrEmail?.Trim();
        var user = await context.Users
            .FirstOrDefaultAsync(
                u => u.Email == usernameOrEmail || u.Username == usernameOrEmail,
                cancellationToken);

        if (user == null)
            return Result<LoginResponse>.FailResult(Error.Unauthorized("Invalid username/email or password"));

        var password = request.Password!.Trim();

        if (!passwordHasher.VerifyPassword(password, user.PasswordHash!))
            return Result<LoginResponse>.FailResult(Error.Unauthorized("Invalid username/email or password"));

        var refreshToken = jwtService.GenerateRefreshToken();

        context.RefreshTokens.Add(
            new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            });

        await context.SaveChangesAsync(cancellationToken);

        var response = jwtService.GenerateToken(user);

        response.Data?.RefreshToken = refreshToken;

        return response;
    }

    public async Task<Result> LogoutAsync(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        var token = await context.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.Token == request.RefreshToken,
                    cancellationToken);

        if (token == null)
            return Result.FailResult(Error.NotFound("Refresh token not found"));

        token.RevokedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        return Result.SuccessResult();
    }

    public async Task<Result<LoginResponse>> RefreshAccessTokenAsync(
        RefreshAccessTokenCommand request,
        CancellationToken cancellationToken)
    {
        var storedToken = await context.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.Token == request.RefreshToken,
                    cancellationToken);

        // if does not have storedToken
        if (storedToken == null)
            return Result<LoginResponse>.FailResult(Error.NotFound("Refresh token not found"));

        // if token has been revoked
        if (storedToken.RevokedAt.HasValue)
            return Result<LoginResponse>.FailResult(Error.Unauthorized("Refresh token has been revoked."));

        // if expiryDate smaller than now
        if (storedToken.ExpiresAt < DateTime.UtcNow)
            return Result<LoginResponse>.FailResult(Error.Unauthorized("Refresh token has been expired."));

        var user = await context.Users
            .FirstOrDefaultAsync(
                u => u.Id == storedToken.UserId,
                cancellationToken);

        if (user == null)
            return Result<LoginResponse>.FailResult(Error.NotFound("User not found."));

        // revoked old token
        storedToken.RevokedAt = DateTime.UtcNow;

        // generate new refresh token
        var newRefreshToken = jwtService.GenerateRefreshToken();

        context.RefreshTokens.Add(
            new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            });

        await context.SaveChangesAsync(cancellationToken);

        var response = jwtService.GenerateToken(user);

        response.Data?.RefreshToken = newRefreshToken;

        return response;
    }

    public async Task<Result<UserResponse>> RegisterAsync(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email?.Trim();
        var existingUserByEmail = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == email,
                cancellationToken);

        if (existingUserByEmail != null)
            return Result<UserResponse>.FailResult(Error.Conflict("Email is already in use."));

        var username = request.Username?.Trim();
        var existingUserByUsername = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Username == username,
                cancellationToken);

        if (existingUserByUsername != null)
            return Result<UserResponse>.FailResult(Error.Conflict("Username is already in use."));

        var user = new User
        {
            FirstName = request.FirstName?.Trim(),
            LastName = request.LastName?.Trim(),
            Username = request.Username?.Trim(),
            Email = email,
            PasswordHash = passwordHasher.HashPassword(request.Password),
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return Result<UserResponse>.SuccessResult(new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email
        });
    }
}
