using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Interfaces.FoodRecords;
using MediatR;

namespace FoodCollectionsBackend.Application.Features.FoodRecords.Queries;

public sealed record GetFoodRecordByIdQuery(Guid Id) : IRequest<Result<FoodRecordDetailResponse?>>;

public class GetFoodRecordByIdQueryHandler(IFoodRecordQueryService foodRecordQueryService) : IRequestHandler<GetFoodRecordByIdQuery, Result<FoodRecordDetailResponse?>>
{
    public async Task<Result<FoodRecordDetailResponse?>> Handle(GetFoodRecordByIdQuery request, CancellationToken cancellationToken)
    => await foodRecordQueryService.GetFoodRecordByIdAsync(request, cancellationToken);
}