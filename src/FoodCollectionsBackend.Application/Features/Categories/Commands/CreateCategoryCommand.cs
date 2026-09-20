using MediatR;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Interfaces.Categories;
namespace FoodCollectionsBackend.Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result<CategoryResponse?>>
{
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public string? ColorHex { get; set; }
}

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.");
    }
}

public class CreateCategoryCommandHandler(ICategoryCommandService categoryCommandService)
    : IRequestHandler<CreateCategoryCommand, Result<CategoryResponse?>>
{
    public async Task<Result<CategoryResponse?>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    => await categoryCommandService.CreateCategoryAsync(request, cancellationToken);
}
