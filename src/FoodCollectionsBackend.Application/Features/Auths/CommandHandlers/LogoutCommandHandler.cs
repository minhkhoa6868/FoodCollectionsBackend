using FoodCollectionsBackend.Application.Interfaces.Auths;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Common.Models;
namespace FoodCollectionsBackend.Application.Features.Auths.CommandHandlers;

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
