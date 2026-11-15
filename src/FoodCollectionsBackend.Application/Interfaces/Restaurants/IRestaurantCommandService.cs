using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Commands;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
namespace FoodCollectionsBackend.Application.Interfaces.Restaurants;

public interface IRestaurantCommandService
{
    Task<Result<RestaurantDetailResponse?>> CreateRestaurantAsync(CreateRestaurantCommand request, CancellationToken cancellationToken);
    Task<Result<RestaurantDetailResponse?>> UpdateRestaurantAsync(UpdateRestaurantCommand request, CancellationToken cancellationToken);
    Task<Result> DeleteRestaurantAsync(DeleteRestaurantCommand request, CancellationToken cancellationToken);
}
