using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Queries;
namespace FoodCollectionsBackend.Application.Interfaces.Restaurants;

public interface IRestaurantQueryService
{
    Task<Result<PaginationResponse<RestaurantSearchResponse>?>> GetRestaurantSearchAsync(GetRestaurantSearchQuery request, CancellationToken cancellationToken);
    Task<Result<RestaurantDetailResponse?>> GetRestaurantByIdAsync(GetRestaurantByIdQuery request, CancellationToken cancellationToken);
}
