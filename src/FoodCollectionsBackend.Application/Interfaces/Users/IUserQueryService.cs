using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Users.Models;
using FoodCollectionsBackend.Application.Features.Users.Queries;
namespace FoodCollectionsBackend.Application.Interfaces.Users;

public interface IUserQueryService
{
    Task<Result<UserResponse?>> GetUserByIdAsync(GetUserByIdQuery request, CancellationToken cancellationToken);
}
