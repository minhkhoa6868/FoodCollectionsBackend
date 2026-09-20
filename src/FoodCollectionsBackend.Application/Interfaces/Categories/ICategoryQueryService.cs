using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Features.Categories.Queries;
namespace FoodCollectionsBackend.Application.Interfaces.Categories;

public interface ICategoryQueryService
{
    Task<Result<PaginationResponse<CategoryResponse>?>> GetCategorySearchAsync(GetCategorySearchQuery request, CancellationToken cancellationToken);
    Task<Result<CategoryResponse?>> GetCategoryByIdAsync(GetCategoryByIdQuery request, CancellationToken cancellationToken);
}
