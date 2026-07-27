using FoodCollectionsBackend.Application.Common.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Interfaces.Auths;
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

public class LogoutCommandHandler(IAuthCommandService authCommandService)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        return await authCommandService.LogoutAsync(request, cancellationToken);
    }
}
