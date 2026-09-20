using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Interfaces.Categories;
using MediatR;

namespace FoodCollectionsBackend.Application.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryResponse?>>;

public class GetCategoryByIdQueryHandler(ICategoryQueryService categoryQueryService) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse?>>
{
    public async Task<Result<CategoryResponse?>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    => await categoryQueryService.GetCategoryByIdAsync(request, cancellationToken);
}
