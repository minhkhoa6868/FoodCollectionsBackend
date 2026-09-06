using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Users.Commands;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
namespace FoodCollectionsBackend.Infrastructure.Services.Users;

public class UserCommandService(IAppDbContext context, ICacheService cacheService) : IUserCommandService
{
    public async Task<Result> UpdateUserAsync(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Id == request.Id &&
                !u.IsDeleted,
                cancellationToken);

        if (user == null)
            return Result.FailResult(Error.NotFound("User not found"));

        user.FirstName = request.FirstName?.Trim();
        user.LastName = request.LastName?.Trim();

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveAsync($"user:{user.Id}", cancellationToken);
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            return Result.FailResult(Error.Invalid(ex.Message));
        }
    }
}
