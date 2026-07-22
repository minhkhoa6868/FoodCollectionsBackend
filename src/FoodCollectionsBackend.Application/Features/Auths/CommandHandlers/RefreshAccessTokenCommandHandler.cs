using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Application.Models.Auths;
namespace FoodCollectionsBackend.Application.Features.Auths.CommandHandlers;

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
