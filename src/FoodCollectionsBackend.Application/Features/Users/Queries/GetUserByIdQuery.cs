using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Users.Models;
using FoodCollectionsBackend.Application.Interfaces.Users;

namespace FoodCollectionsBackend.Application.Features.Users.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<Result<UserResponse?>>;

public class GetUserByIdQueryHandler(IUserQueryService userQueryService) : IRequestHandler<GetUserByIdQuery, Result<UserResponse?>>
{
    public async Task<Result<UserResponse?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    => await userQueryService.GetUserByIdAsync(request, cancellationToken);
}
