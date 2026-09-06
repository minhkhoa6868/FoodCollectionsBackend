using FoodCollectionsBackend.Domain.Entities;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Auths.Models;
namespace FoodCollectionsBackend.Application.Interfaces;

public interface IJwtService
{
    Result<LoginResponse> GenerateToken(User user);
    string GenerateRefreshToken();
}
