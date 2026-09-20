using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Interfaces.Categories;
using MediatR;

namespace FoodCollectionsBackend.Application.Features.Categories.Queries;

public class GetCategorySearchQuery : PaginationRequest, IRequest<Result<PaginationResponse<CategoryResponse>?>>;

public class GetCategorySearchQueryHandler(ICategoryQueryService categoryQueryService) : IRequestHandler<GetCategorySearchQuery, Result<PaginationResponse<CategoryResponse>?>>
{
    public async Task<Result<PaginationResponse<CategoryResponse>?>> Handle(GetCategorySearchQuery request, CancellationToken cancellationToken)
    => await categoryQueryService.GetCategorySearchAsync(request, cancellationToken);
}
