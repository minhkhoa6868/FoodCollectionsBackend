using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Models.Auths;
using FluentValidation;

namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record LoginCommand(
    string UsernameOrEmail,
    string Password
) : IRequest<Result<LoginResponse>>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("Username or email is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
