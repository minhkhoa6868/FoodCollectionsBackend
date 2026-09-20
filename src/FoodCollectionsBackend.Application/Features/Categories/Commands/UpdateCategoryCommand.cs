using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Interfaces.Categories;
using MediatR;
using FluentValidation;

namespace FoodCollectionsBackend.Application.Features.Categories.Commands;

public class UpdateCategoryCommand : CreateCategoryCommand, IRequest<Result<CategoryResponse?>>
{
    public Guid Id { get; set; }
}

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category ID is required.");
        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("Category name is required.");
    }
}

public class UpdateCategoryCommandHandler(ICategoryCommandService categoryCommandService)
    : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponse?>>
{
    public async Task<Result<CategoryResponse?>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    => await categoryCommandService.UpdateCategoryAsync(request, cancellationToken);
}
