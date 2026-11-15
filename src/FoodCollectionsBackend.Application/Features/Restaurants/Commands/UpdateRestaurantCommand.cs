using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;

namespace FoodCollectionsBackend.Application.Features.Restaurants.Commands;

public class UpdateRestaurantCommand : CreateRestaurantCommand, IRequest<Result<RestaurantDetailResponse?>>
{
    public Guid Id { get; set; }
}

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Restaurant ID is required.");
    }
}

public class UpdateRestaurantCommandHandler(IRestaurantCommandService restaurantCommandService) : IRequestHandler<UpdateRestaurantCommand, Result<RestaurantDetailResponse?>>
{
    public async Task<Result<RestaurantDetailResponse?>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    => await restaurantCommandService.UpdateRestaurantAsync(request, cancellationToken);
}
