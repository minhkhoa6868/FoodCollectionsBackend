using FoodCollectionsBackend.Application.Interfaces.Data;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;
using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Commands;
using FoodCollectionsBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;
namespace FoodCollectionsBackend.Infrastructure.Services.Restaurants;

public class RestaurantCommandService(
    IAppDbContext context,
    ICurrentUser currentUser,
    ICacheService cacheService) : IRestaurantCommandService
{
    public async Task<Result<RestaurantDetailResponse?>> CreateRestaurantAsync(
        CreateRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var restaurant = new Restaurant
        {
            RestaurantName = !string.IsNullOrWhiteSpace(request.RestaurantName) ? request.RestaurantName.Trim() : null,
            MinPrice = request.MinPrice,
            MaxPrice = request.MaxPrice,
            Address = !string.IsNullOrWhiteSpace(request.Address) ? request.Address.Trim() : null,
            PlaceId = !string.IsNullOrWhiteSpace(request.PlaceId) ? request.PlaceId.Trim() : null,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsVisited = request.IsVisited,
            VisitedDate = request.VisitedDate,
            Notes = !string.IsNullOrWhiteSpace(request.Notes) ? request.Notes.Trim() : null,
            CategoryId = request.CategoryId,
        };

        try
        {
            await context.Restaurants.AddAsync(restaurant, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result<RestaurantDetailResponse?>.SuccessResult(restaurant.Adapt<RestaurantDetailResponse>());
        }
        catch (Exception ex)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result<RestaurantDetailResponse?>> UpdateRestaurantAsync(
        UpdateRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var restaurant = await context.Restaurants
            .FirstOrDefaultAsync(r =>
                r.Id == request.Id,
                cancellationToken);

        if (restaurant == null)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.NotFound("Restaurant not found."));
        }

        if (!string.IsNullOrWhiteSpace(request.RestaurantName) && request.RestaurantName.Trim() != restaurant.RestaurantName)
        {
            restaurant.RestaurantName = request.RestaurantName.Trim();
        }

        if (request.MinPrice.HasValue && request.MinPrice != restaurant.MinPrice)
        {
            restaurant.MinPrice = request.MinPrice;
        }

        if (request.MaxPrice.HasValue && request.MaxPrice != restaurant.MaxPrice)
        {
            restaurant.MaxPrice = request.MaxPrice;
        }

        if (!string.IsNullOrWhiteSpace(request.Address) && request.Address.Trim() != restaurant.Address)
        {
            restaurant.Address = request.Address.Trim();
        }

        if (!string.IsNullOrWhiteSpace(request.PlaceId) && request.PlaceId.Trim() != restaurant.PlaceId)
        {
            restaurant.PlaceId = request.PlaceId.Trim();
        }

        if (request.Latitude.HasValue && request.Latitude != restaurant.Latitude)
        {
            restaurant.Latitude = request.Latitude;
        }

        if (request.Longitude.HasValue && request.Longitude != restaurant.Longitude)
        {
            restaurant.Longitude = request.Longitude;
        }

        if (request.IsVisited.HasValue && request.IsVisited != restaurant.IsVisited)
        {
            restaurant.IsVisited = request.IsVisited;
        }

        if (request.VisitedDate.HasValue && request.VisitedDate != restaurant.VisitedDate)
        {
            restaurant.VisitedDate = request.VisitedDate;
        }

        if (!string.IsNullOrWhiteSpace(request.Notes) && request.Notes.Trim() != restaurant.Notes)
        {
            restaurant.Notes = request.Notes.Trim();
        }

        if (request.CategoryId != restaurant.CategoryId)
        {
            restaurant.CategoryId = request.CategoryId;
        }

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await cacheService.RemoveAsync($"Restaurant_{restaurant.Id}", cancellationToken);
            return Result<RestaurantDetailResponse?>.SuccessResult(restaurant.Adapt<RestaurantDetailResponse>());
        }
        catch (Exception ex)
        {
            return Result<RestaurantDetailResponse?>.FailResult(Error.Invalid(ex.Message));
        }
    }

    public async Task<Result> DeleteRestaurantAsync(
        DeleteRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser?.UserId == null)
        {
            return Result.FailResult(Error.Unauthorized("User is not authenticated."));
        }

        var restaurants = await context.Restaurants
            .Where(r => request.Ids.Contains(r.Id) && !r.IsDeleted)
            .ToListAsync(cancellationToken);

        if (restaurants.Count == 0)
        {
            return Result.FailResult(Error.NotFound("No restaurants found to delete."));
        }

        try
        {
            context.Restaurants.RemoveRange(restaurants);
            await context.SaveChangesAsync(cancellationToken);
            foreach (var restaurant in restaurants)
            {
                await cacheService.RemoveAsync($"Restaurant_{restaurant.Id}", cancellationToken);
            }
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            return Result.FailResult(Error.Invalid(ex.Message));
        }
    }
}
