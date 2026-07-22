using FoodCollectionsBackend.Domain.Entities;
using FoodCollectionsBackend.Application.Models.Auths;
using FoodCollectionsBackend.Application.Common.Models;
namespace FoodCollectionsBackend.Application.Interfaces;

public interface IJwtService
{
    Result<LoginResponse> GenerateToken(User user);
    string GenerateRefreshToken();
}
