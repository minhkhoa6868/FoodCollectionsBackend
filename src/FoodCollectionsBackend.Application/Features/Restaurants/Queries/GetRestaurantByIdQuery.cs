using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;

namespace FoodCollectionsBackend.Application.Features.Restaurants.Queries;

public sealed record GetRestaurantByIdQuery(Guid Id) : IRequest<Result<RestaurantDetailResponse?>>;

public class GetRestaurantByIdQueryHandler(IRestaurantQueryService restaurantQueryService) : IRequestHandler<GetRestaurantByIdQuery, Result<RestaurantDetailResponse?>>
{
    public async Task<Result<RestaurantDetailResponse?>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    => await restaurantQueryService.GetRestaurantByIdAsync(request, cancellationToken);
}
