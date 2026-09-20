using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Commands;
namespace FoodCollectionsBackend.Application.Interfaces.Categories;

public interface ICategoryCommandService
{
    Task<Result<CategoryResponse?>> CreateCategoryAsync(CreateCategoryCommand request, CancellationToken cancellationToken);
    Task<Result<CategoryResponse?>> UpdateCategoryAsync(UpdateCategoryCommand request, CancellationToken cancellationToken);
    Task<Result> DeleteCategoryAsync(DeleteCategoryCommand request, CancellationToken cancellationToken);
}
