using FluentValidation;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Interfaces.FoodRecords;
using MediatR;
namespace FoodCollectionsBackend.Application.Features.FoodRecords.Commands;

public class CreateFoodRecordCommand : IRequest<Result<FoodRecordDetailResponse?>>
{
    public string? FoodName { get; set; }
    public decimal? Price { get; set; }
    public Guid RestaurantId { get; set; }
    public decimal? Rating { get; set; }
    public bool? IsTried { get; set; } = false;
    public string? Notes { get; set; }
    // public ICollection<FoodImageRequest>? FoodImages { get; set; } = new List<FoodImageRequest>();
}

public class CreateFoodRecordCommandValidator : AbstractValidator<CreateFoodRecordCommand>
{
    public CreateFoodRecordCommandValidator()
    {
        RuleFor(x => x.FoodName).NotEmpty().WithMessage("Food name is required.");
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
        RuleFor(x => x.RestaurantId).NotEmpty().WithMessage("Restaurant ID is required.");
        RuleFor(x => x.Rating).InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");
    }
}

public class CreateFoodRecordCommandHandler(IFoodRecordCommandService foodRecordCommandService) : IRequestHandler<CreateFoodRecordCommand, Result<FoodRecordDetailResponse?>>
{
    public async Task<Result<FoodRecordDetailResponse?>> Handle(CreateFoodRecordCommand request, CancellationToken cancellationToken)
    => await foodRecordCommandService.CreateFoodRecordAsync(request, cancellationToken);
}
