using FoodCollectionsBackend.Application.Features.Auths.Commands;
using Serilog;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Users.Models;
using FoodCollectionsBackend.Application.Features.Auths.Models;
namespace FoodCollectionsBackend.Application.Interfaces.Auths;

public interface IAuthCommandService
{
    Task<Result<LoginResponse>> LoginAsync(LoginCommand request, CancellationToken cancellationToken);
    Task<Result> LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> RefreshAccessTokenAsync(RefreshAccessTokenCommand request, CancellationToken cancellationToken);
    Task<Result<UserResponse>> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken);
    Task<Result> SendOtpAsync(SendOtpCommand request, CancellationToken cancellationToken);
    Task<Result<VerifiedOtpResponse?>> VerifiedOtpAsync(VerifiedOtpCommand request, CancellationToken cancellationToken);
    Task<Result> ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken);
}
