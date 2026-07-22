using Microsoft.AspNetCore.Mvc;
using MediatR;
using Serilog;
using FoodCollectionsBackend.Application.Features.Auths.Commands;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Models.Auths;
using Microsoft.AspNetCore.Authorization;
using FoodCollectionsBackend.Application.Models.Users;
using System.ComponentModel;
namespace FoodCollectionsBackend.API.Controller;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Login with username and password
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns> <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Logout the current user
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(LogoutCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Refresh the access token using a refresh token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(Result<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshAccessToken(RefreshAccessTokenCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Result<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
