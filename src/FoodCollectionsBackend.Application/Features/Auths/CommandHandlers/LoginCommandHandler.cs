using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Application.Common.Models;
namespace FoodCollectionsBackend.Application.Features.Auths.CommandHandlers;

public class LoginCommandHandler(IAuthCommandService authCommandService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        return await authCommandService.LoginAsync(request, cancellationToken);
    }
}
