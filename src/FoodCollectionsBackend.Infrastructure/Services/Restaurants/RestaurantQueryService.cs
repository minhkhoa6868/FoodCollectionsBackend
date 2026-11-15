using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Domain.Entities;
using FoodCollectionsBackend.Application.Features.Restaurants.Queries;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FoodCollectionsBackend.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using FoodCollectionsBackend.Application.Common.Extentions;
using Mapster;
namespace FoodCollectionsBackend.Infrastructure.Services.Restaurants;

public class RestaurantQueryService(
    IAppDbContext context,
    ICurrentUser currentUser,
    ICacheService cacheService) : IRestaurantQueryService
{
    public async Task<Result<PaginationResponse<RestaurantSearchResponse>?>> GetRestaurantSearchAsync(
        GetRestaurantSearchQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<PaginationResponse<RestaurantSearchResponse>?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var query = context.Restaurants
            .AsNoTracking()
            .Where(r =>
                r.CategoryId == request.CategoryId &&
                !r.IsDeleted);

        // Filter by search string
        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.Trim().ToLower();
            query = query.Where(r =>
                !string.IsNullOrWhiteSpace(r.RestaurantName) &&
                r.RestaurantName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
        }

        var restaurants = query
            .ApplySorting(
                request.SortBy,
                request.SortDirection,
                "RestaurantName",
                ["RestaurantName", "Address", "MinPrice", "MaxPrice"])
            .Select(r => new RestaurantSearchResponse
            {
                Id = r.Id,
                RestaurantName = r.RestaurantName,
                IsVisited = r.IsVisited,
                NumberOfFoodRecords = r.FoodRecords.Count(fr => !fr.IsDeleted)
            });

        var result = await PaginationResponse<RestaurantSearchResponse>.ToPagedResultAsync(
            restaurants,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return Result<PaginationResponse<RestaurantSearchResponse>?>.SuccessResult(result);
    }

    public async Task<Result<RestaurantDetailResponse?>> GetRestaurantByIdAsync(
        GetRestaurantByIdQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        // Check cache first
        var cacheKey = $"Restaurant_{request.Id}";
        var cachedRestaurant = await cacheService.GetAsync<RestaurantDetailResponse>(cacheKey, cancellationToken);
        if (cachedRestaurant != null)
        {
            return Result<RestaurantDetailResponse?>.SuccessResult(cachedRestaurant);
        }

        var restaurant = await context.Restaurants
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.Id == request.Id &&
                !r.IsDeleted,
                cancellationToken);

        if (restaurant == null)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.NotFound("Restaurant not found."));
        }

        var response = restaurant.Adapt<RestaurantDetailResponse>();

        // Cache the result
        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(30), cancellationToken);

        return Result<RestaurantDetailResponse?>.SuccessResult(response);
    }
}
