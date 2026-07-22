using FoodCollectionsBackend.Application.Features.Auths.Commands;
using Serilog;
using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Models.Users;
namespace FoodCollectionsBackend.Application.Interfaces.Auths;

public interface IAuthCommandService
{
    Task<Result<LoginResponse>> LoginAsync(LoginCommand request, CancellationToken cancellationToken);
    Task<Result> LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> RefreshAccessTokenAsync(RefreshAccessTokenCommand request, CancellationToken cancellationToken);
    Task<Result<UserResponse>> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken);
}
