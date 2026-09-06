using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FluentValidation;
using FoodCollectionsBackend.Application.Features.Users.Models;
namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password
) : IRequest<Result<UserResponse>>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(20).WithMessage("Username must not exceed 20 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$").WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
    }
}

public class RegisterCommandHandler(IAuthCommandService authCommandService)
    : IRequestHandler<RegisterCommand, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    => await authCommandService.RegisterAsync(request, cancellationToken);
}
