using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Interfaces.FoodRecords;
using MediatR;
using FluentValidation;

namespace FoodCollectionsBackend.Application.Features.FoodRecords.Queries;

public class GetFoodRecordSearchQuery : CursorRequest, IRequest<Result<CursorResponse<FoodRecordSearchResponse>?>>
{
    public Guid? RestaurantId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class GetFoodRecordSearchQueryValidator : AbstractValidator<GetFoodRecordSearchQuery>
{
    public GetFoodRecordSearchQueryValidator()
    {
        RuleFor(x => x.RestaurantId).NotEmpty().WithMessage("RestaurantId is required.");
    }
}

public class GetFoodRecordSearchQueryHandler(IFoodRecordQueryService foodRecordQueryService)
    : IRequestHandler<GetFoodRecordSearchQuery, Result<CursorResponse<FoodRecordSearchResponse>?>>
{
    public async Task<Result<CursorResponse<FoodRecordSearchResponse>?>> Handle(GetFoodRecordSearchQuery request, CancellationToken cancellationToken)
    => await foodRecordQueryService.GetFoodRecordSearchAsync(request, cancellationToken);
}
