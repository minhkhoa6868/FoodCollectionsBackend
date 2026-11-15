using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FoodCollectionsBackend.Application.Interfaces.Restaurants;
using MediatR;
using FluentValidation;

namespace FoodCollectionsBackend.Application.Features.Restaurants.Queries;

public class GetRestaurantSearchQuery : PaginationRequest, IRequest<Result<PaginationResponse<RestaurantSearchResponse>?>>
{
    public Guid? CategoryId { get; set; }
}

public class GetRestaurantSearchQueryValidator : AbstractValidator<GetRestaurantSearchQuery>
{
    public GetRestaurantSearchQueryValidator()
    {
        RuleFor(x => x.CategoryId).NotNull().WithMessage("Category ID is required.");
    }
}

public class GetRestaurantSearchQueryHandler(IRestaurantQueryService restaurantQueryService) : IRequestHandler<GetRestaurantSearchQuery, Result<PaginationResponse<RestaurantSearchResponse>?>>
{
    public async Task<Result<PaginationResponse<RestaurantSearchResponse>?>> Handle(GetRestaurantSearchQuery request, CancellationToken cancellationToken)
    => await restaurantQueryService.GetRestaurantSearchAsync(request, cancellationToken);
}
