using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Queries;
namespace FoodCollectionsBackend.Application.Interfaces.FoodRecords;

public interface IFoodRecordQueryService
{
    Task<Result<CursorResponse<FoodRecordSearchResponse>?>> GetFoodRecordSearchAsync(GetFoodRecordSearchQuery request, CancellationToken cancellationToken);
    Task<Result<FoodRecordDetailResponse?>> GetFoodRecordByIdAsync(GetFoodRecordByIdQuery request, CancellationToken cancellationToken);
}
