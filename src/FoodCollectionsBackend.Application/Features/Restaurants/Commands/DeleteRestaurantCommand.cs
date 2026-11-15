using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;

namespace FoodCollectionsBackend.Application.Features.Restaurants.Commands;

public sealed record DeleteRestaurantCommand(Guid[] Ids) : IRequest<Result>;

public class DeleteRestaurantCommandHandler(IRestaurantCommandService restaurantCommandService) : IRequestHandler<DeleteRestaurantCommand, Result>
{
    public async Task<Result> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    => await restaurantCommandService.DeleteRestaurantAsync(request, cancellationToken);
}