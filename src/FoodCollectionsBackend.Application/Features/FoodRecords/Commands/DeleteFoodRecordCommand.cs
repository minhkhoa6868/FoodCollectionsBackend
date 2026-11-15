using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.FoodRecords;

namespace FoodCollectionsBackend.Application.Features.FoodRecords.Commands;

public sealed record DeleteFoodRecordCommand(Guid[] Ids) : IRequest<Result>;

public class DeleteFoodRecordCommandHandler(IFoodRecordCommandService foodRecordCommandService) : IRequestHandler<DeleteFoodRecordCommand, Result>
{
    public async Task<Result> Handle(DeleteFoodRecordCommand request, CancellationToken cancellationToken)
    => await foodRecordCommandService.DeleteFoodRecordAsync(request, cancellationToken);
}
