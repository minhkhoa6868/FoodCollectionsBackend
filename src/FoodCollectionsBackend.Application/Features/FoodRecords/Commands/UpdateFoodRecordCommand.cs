using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Interfaces.FoodRecords;
namespace FoodCollectionsBackend.Application.Features.FoodRecords.Commands;

public class UpdateFoodRecordCommand : CreateFoodRecordCommand, IRequest<Result<FoodRecordDetailResponse?>>
{
    public Guid Id { get; set; }
}

public class UpdateFoodRecordCommandHandler(IFoodRecordCommandService foodRecordCommandService) : IRequestHandler<UpdateFoodRecordCommand, Result<FoodRecordDetailResponse?>>
{
    public async Task<Result<FoodRecordDetailResponse?>> Handle(UpdateFoodRecordCommand request, CancellationToken cancellationToken)
    => await foodRecordCommandService.UpdateFoodRecordAsync(request, cancellationToken);
}
