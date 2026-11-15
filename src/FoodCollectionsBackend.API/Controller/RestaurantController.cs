using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodCollectionsBackend.Application.Features.Restaurants.Queries;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Models;
using FoodCollectionsBackend.Application.Features.Restaurants.Commands;

namespace FoodCollectionsBackend.API.Controller;

[ApiController]
[Authorize]
[Route("api/restaurants")]
public class RestaurantController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [EndpointDescription("Get restaurant search with pagination")]
    [ProducesResponseType(typeof(Result<PaginationResponse<RestaurantSearchResponse>?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRestaurantSearch([FromQuery] GetRestaurantSearchQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [EndpointDescription("Get restaurant by ID")]
    [ProducesResponseType(typeof(Result<RestaurantDetailResponse?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRestaurantById(Guid id)
    {
        var result = await mediator.Send(new GetRestaurantByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [EndpointDescription("Create a new restaurant")]
    [ProducesResponseType(typeof(Result<RestaurantDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    [EndpointDescription("Update an existing restaurant")]
    [ProducesResponseType(typeof(Result<RestaurantDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateRestaurant([FromBody] UpdateRestaurantCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [EndpointDescription("Delete a restaurant by ID")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteRestaurant(Guid id)
    {
        var result = await mediator.Send(new DeleteRestaurantCommand([id]));
        return Ok(result);
    }

    [HttpDelete]
    [EndpointDescription("Delete multiple restaurants by IDs")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteRestaurants([FromBody] DeleteRestaurantCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
