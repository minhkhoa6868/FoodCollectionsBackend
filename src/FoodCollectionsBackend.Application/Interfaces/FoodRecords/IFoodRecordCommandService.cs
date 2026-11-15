using FoodCollectionsBackend.Application.Features.FoodRecords.Commands;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
namespace FoodCollectionsBackend.Application.Interfaces.FoodRecords;

public interface IFoodRecordCommandService
{
    Task<Result<FoodRecordDetailResponse?>> CreateFoodRecordAsync(CreateFoodRecordCommand request, CancellationToken cancellationToken);
    Task<Result<FoodRecordDetailResponse?>> UpdateFoodRecordAsync(UpdateFoodRecordCommand request, CancellationToken cancellationToken);
    Task<Result> DeleteFoodRecordAsync(DeleteFoodRecordCommand request, CancellationToken cancellationToken);
}
