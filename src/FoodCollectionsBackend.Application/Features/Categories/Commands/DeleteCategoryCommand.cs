using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Categories;

namespace FoodCollectionsBackend.Application.Features.Categories.Commands;

public sealed record DeleteCategoryCommand(Guid[] Ids) : IRequest<Result>;

public class DeleteCategoryCommandHandler(ICategoryCommandService categoryCommandService) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    => await categoryCommandService.DeleteCategoryAsync(request, cancellationToken);
}
