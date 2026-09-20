using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Application.Interfaces.Categories;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Queries;
using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Application.Common.Extentions;
namespace FoodCollectionsBackend.Infrastructure.Services.Categories;

public class CategoryQueryService(
    IAppDbContext context,
    ICurrentUser currentUser,
    ICacheService cacheService) : ICategoryQueryService
{
    public async Task<Result<PaginationResponse<CategoryResponse>?>> GetCategorySearchAsync(
        GetCategorySearchQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<PaginationResponse<CategoryResponse>?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var query = context.Categories
            .AsNoTracking()
            .Where(c =>
                c.UserId == (Guid)currentUser.UserId &&
                !c.IsDeleted);

        // Filter by search string
        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.Trim().ToLower();
            query = query.Where(c =>
                !string.IsNullOrWhiteSpace(c.CategoryName) &&
                c.CategoryName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
        }

        // Get response with pagination
        var categories = query
            .ApplySorting(
                request.SortBy,
                request.SortDirection,
                "CategoryName",
                ["CategoryName", "Description", "ColorHex"])
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Description = c.Description,
                ColorHex = c.ColorHex
            });

        var result = await PaginationResponse<CategoryResponse>.ToPagedResultAsync(
            categories,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return Result<PaginationResponse<CategoryResponse>?>.SuccessResult(result);
    }

    public async Task<Result<CategoryResponse?>> GetCategoryByIdAsync(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<CategoryResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        // Check cache first
        var cacheKey = $"Category_{request.Id}";
        var cachedCategory = await cacheService.GetAsync<CategoryResponse>(cacheKey, cancellationToken);
        if (cachedCategory != null)
        {
            return Result<CategoryResponse?>.SuccessResult(cachedCategory);
        }

        // Find category
        var category = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c =>
                c.Id == request.Id &&
                c.UserId == (Guid)currentUser.UserId &&
                !c.IsDeleted,
                cancellationToken);

        if (category == null)
        {
            return Result<CategoryResponse?>.FailResult(Error.NotFound("Category not found."));
        }

        var categoryResponse = new CategoryResponse
        {
            Id = category.Id,
            CategoryName = category.CategoryName,
            Description = category.Description,
            ColorHex = category.ColorHex
        };

        // Cache the result
        await cacheService.SetAsync(cacheKey, categoryResponse, TimeSpan.FromMinutes(30), cancellationToken);

        return Result<CategoryResponse?>.SuccessResult(categoryResponse);
    }
}
