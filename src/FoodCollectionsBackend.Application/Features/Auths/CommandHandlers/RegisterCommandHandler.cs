using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Application.Models.Users;
namespace FoodCollectionsBackend.Application.Features.Auths.CommandHandlers;

public class RegisterCommandHandler(IAuthCommandService authCommandService)
    : IRequestHandler<RegisterCommand, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return await authCommandService.RegisterAsync(request, cancellationToken);
    }
}
