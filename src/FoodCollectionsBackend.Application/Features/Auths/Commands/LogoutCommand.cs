using FoodCollectionsBackend.Application.Common.Models;
using FluentValidation;
namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record LogoutCommand(
    string RefreshToken
) : IRequest<Result>;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
