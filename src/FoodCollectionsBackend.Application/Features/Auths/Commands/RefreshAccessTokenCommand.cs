using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Application.Common.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Interfaces.Auths;
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

public class RefreshAccessTokenCommandHandler(IAuthCommandService authCommandService)
    : IRequestHandler<RefreshAccessTokenCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        RefreshAccessTokenCommand request,
        CancellationToken cancellationToken)
    {
        return await authCommandService.RefreshAccessTokenAsync(request, cancellationToken);
    }
}
