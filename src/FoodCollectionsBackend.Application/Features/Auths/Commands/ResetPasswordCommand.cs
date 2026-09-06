using FluentValidation;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using System.Data;
namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record ResetPasswordCommand(
    string ResetToken,
    string Password
) : IRequest<Result>;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.ResetToken)
            .NotEmpty().WithMessage("Reset token is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}

public class ResetPasswordCommmandHandler(IAuthCommandService authCommandService) : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    => await authCommandService.ResetPasswordAsync(request, cancellationToken);
}
