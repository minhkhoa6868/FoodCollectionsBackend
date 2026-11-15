using FoodCollectionsBackend.Application.Interfaces.Users;
using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Features.Users.Queries;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Users.Models;
using FoodCollectionsBackend.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mapster;
namespace FoodCollectionsBackend.Infrastructure.Services.Users;

public class UserQueryService(IAppDbContext context, ICacheService cacheService) : IUserQueryService
{
    public async Task<Result<UserResponse?>> GetUserByIdAsync(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"User_{request.Id}";

        // check cache first
        var cached = await cacheService.GetAsync<UserResponse>(cacheKey, cancellationToken);
        if (cached is not null)
            return Result<UserResponse?>.SuccessResult(cached);

        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Id == request.Id &&
                !u.IsDeleted,
                cancellationToken);

        if (user == null)
            return Result<UserResponse?>.FailResult(Error.NotFound("User not found"));

        var response = user.Adapt<UserResponse>();

        // cache the result
        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10), cancellationToken);

        return Result<UserResponse?>.SuccessResult(response);
    }
}
