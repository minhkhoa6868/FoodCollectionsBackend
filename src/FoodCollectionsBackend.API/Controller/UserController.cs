using Microsoft.AspNetCore.Mvc;
using MediatR;
using FoodCollectionsBackend.Application.Features.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Features.Users.Models;
using FoodCollectionsBackend.Application.Features.Users.Queries;
namespace FoodCollectionsBackend.API.Controller;

[ApiController]
[Route("api/users")]
public class UserController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [EndpointDescription("Get user by id")]
    [Authorize]
    [ProducesResponseType(typeof(Result<UserResponse?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut]
    [EndpointDescription("Update user profile")]
    [Authorize]
    [ProducesResponseType(typeof(Result<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProfile(UpdateUserCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
