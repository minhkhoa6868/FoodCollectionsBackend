using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;
namespace FoodCollectionsBackend.Application.Features.Restaurants.Commands;

public class CreateRestaurantCommand : IRequest<Result<RestaurantDetailResponse?>>
{
    public string? RestaurantName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Address { get; set; }
    public string? PlaceId { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool? IsVisited { get; set; } = false;
    public DateTime? VisitedDate { get; set; }
    public string? Notes { get; set; }
    public Guid CategoryId { get; set; }
}

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.RestaurantName).NotEmpty().WithMessage("Restaurant name is required.");
        RuleFor(x => x.CategoryId).NotNull().WithMessage("Category ID is required.");
    }
}

public class CreateRestaurantCommandHandler(IRestaurantCommandService restaurantCommandService) : IRequestHandler<CreateRestaurantCommand, Result<RestaurantDetailResponse?>>
{
    public async Task<Result<RestaurantDetailResponse?>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    => await restaurantCommandService.CreateRestaurantAsync(request, cancellationToken);
}
