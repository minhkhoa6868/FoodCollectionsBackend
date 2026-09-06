using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Users.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FoodCollectionsBackend.Application.Interfaces.Users;

public interface IUserCommandService
{
    Task<Result> UpdateUserAsync(UpdateUserCommand request, CancellationToken cancellationToken);
}
