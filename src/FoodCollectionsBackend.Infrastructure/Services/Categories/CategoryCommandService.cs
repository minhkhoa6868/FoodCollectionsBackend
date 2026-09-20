using FoodCollectionsBackend.Application.Interfaces.Categories;
using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Features.Categories.Commands;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Application.Interfaces;
namespace FoodCollectionsBackend.Infrastructure.Services.Categories;

public class CategoryCommandService(
    IAppDbContext context,
    ICurrentUser currentUser,
    ICacheService cacheService) : ICategoryCommandService
{
    public async Task<Result<CategoryResponse?>> CreateCategoryAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<CategoryResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var category = new Category
        {
            CategoryName = request.CategoryName?.Trim(),
            Description = request.Description?.Trim(),
            ColorHex = request.ColorHex?.Trim(),
            UserId = (Guid)currentUser.UserId,
        };

        await context.Categories.AddAsync(category, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result<CategoryResponse?>.SuccessResult(new CategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
                ColorHex = category.ColorHex
            });
        }
        catch (Exception ex)
        {
            return Result<CategoryResponse?>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result<CategoryResponse?>> UpdateCategoryAsync(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<CategoryResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var category = await context.Categories
            .FirstOrDefaultAsync(c =>
                c.Id == request.Id &&
                c.UserId == (Guid)currentUser.UserId &&
                !c.IsDeleted, cancellationToken);

        if (category == null)
        {
            return Result<CategoryResponse?>.FailResult(Error.NotFound("Category not found."));
        }

        if (!string.IsNullOrWhiteSpace(request.CategoryName) && request.CategoryName.Trim() != category.CategoryName)
        {
            category.CategoryName = request.CategoryName.Trim();
        }

        if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Trim() != category.Description)
        {
            category.Description = request.Description.Trim();
        }

        if (!string.IsNullOrWhiteSpace(request.ColorHex) && request.ColorHex.Trim() != category.ColorHex)
        {
            category.ColorHex = request.ColorHex.Trim();
        }

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveAsync($"Category_{category.Id}", cancellationToken);
            return Result<CategoryResponse?>.SuccessResult(new CategoryResponse
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
                ColorHex = category.ColorHex
            });
        }
        catch (Exception ex)
        {
            return Result<CategoryResponse?>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result> DeleteCategoryAsync(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (currentUser?.UserId == null)
            {
                return Result.FailResult(Error.Unauthorized("User is not authenticated."));
            }

            var categories = await context.Categories
                .Where(c =>
                    request.Ids.Contains(c.Id) &&
                    c.UserId == (Guid)currentUser.UserId &&
                    !c.IsDeleted)
                .ToListAsync(cancellationToken);

            if (categories.Count == 0)
            {
                return Result.FailResult(Error.NotFound("No categories found to delete."));
            }

            context.Categories.RemoveRange(categories);
            await context.SaveChangesAsync(cancellationToken);
            foreach (var category in categories)
            {
                await cacheService.RemoveAsync($"Category_{category.Id}", cancellationToken);
            }
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            return Result.FailResult(Error.Invalid(ex.Message));
        }
    }
}
