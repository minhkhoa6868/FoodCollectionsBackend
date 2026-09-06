using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Auths.Models;
using FoodCollectionsBackend.Application.Features.Users.Models;
using FoodCollectionsBackend.Domain.Entities;
using FoodCollectionsBackend.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace FoodCollectionsBackend.Infrastructure.Services.Auths;

public class AuthCommandService(
    IAppDbContext context,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IEmailService emailService,
    OtpService otpService,
    IMemoryCache memoryCache) : IAuthCommandService
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

        try
        {
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
        catch (Exception ex)
        {
            return Result<LoginResponse>.FailResult(Error.Invalid(ex.Message));
        }
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

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            return Result.FailResult(Error.Invalid(ex.Message));
        }
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

        try
        {
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
        catch (Exception ex)
        {
            return Result<LoginResponse>.FailResult(Error.Invalid(ex.Message));
        }
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
            PasswordHash = passwordHasher.HashPassword(request.Password.Trim()),
        };

        try
        {
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
        catch (Exception ex)
        {
            return Result<UserResponse>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result> SendOtpAsync(
        SendOtpCommand request,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                !u.IsDeleted,
                cancellationToken);

        if (user == null)
            return Result.FailResult(Error.NotFound("User not found."));

        // Generate OTP
        var otp = OtpService.GenerateOtp();

        // Store Otp
        otpService.StoreOtp(request.Email, otp);

        // Send OTP via email
        await emailService.SendOtpAsync(request.Email, otp);

        return Result.SuccessResult();
    }

    public async Task<Result<VerifiedOtpResponse?>> VerifiedOtpAsync(
        VerifiedOtpCommand request,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                !u.IsDeleted,
                cancellationToken);

        if (user == null)
            return Result<VerifiedOtpResponse?>.FailResult(Error.NotFound("User not found."));

        // Verify OTP
        var isValidOtp = otpService.VerifyOtp(request.Email, request.Otp);

        if (!isValidOtp)
            return Result<VerifiedOtpResponse?>.FailResult(Error.Unauthorized("Invalid or expired OTP."));

        // Create reset password token
        var resetToken = Guid.NewGuid().ToString();

        // Store the reset token in memory cache with a 15-minute expiration
        memoryCache.Set($"reset:{resetToken}", request.Email, TimeSpan.FromMinutes(15));

        return Result<VerifiedOtpResponse?>.SuccessResult(new VerifiedOtpResponse
        {
            ResetToken = resetToken
        });
    }

    public async Task<Result> ResetPasswordAsync(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (!memoryCache.TryGetValue($"reset:{request.ResetToken}", out string? email))
            return Result.FailResult(Error.Invalid("Invalid or expired reset token"));

        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Email == email &&
                !u.IsDeleted,
                cancellationToken);

        if (user == null)
            return Result.FailResult(Error.NotFound("User not found"));

        user.PasswordHash = passwordHasher.HashPassword(request.Password.Trim());

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            return Result.FailResult(Error.Invalid(ex.Message));
        }
    }
}
