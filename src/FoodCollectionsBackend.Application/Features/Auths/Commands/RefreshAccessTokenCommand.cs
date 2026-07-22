using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Application.Common.Models;
using FluentValidation;
namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record RefreshAccessTokenCommand(
    string RefreshToken
) : IRequest<Result<LoginResponse>>;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
