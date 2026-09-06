using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FluentValidation;
using System.Reflection.Metadata;
using FoodCollectionsBackend.Application.Features.Auths.Models;

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

public class LoginCommandHandler(IAuthCommandService authCommandService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    => await authCommandService.LoginAsync(request, cancellationToken);
}
