using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using FoodCollectionsBackend.Application.Features.FoodRecords.Queries;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Models;
using FoodCollectionsBackend.Application.Features.FoodRecords.Commands;
namespace FoodCollectionsBackend.API.Controller;

[ApiController]
[Authorize]
[Route("api/food-records")]
public class FoodRecordController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [EndpointDescription("Get food record search with pagination")]
    [ProducesResponseType(typeof(Result<CursorResponse<FoodRecordSearchResponse>?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFoodRecordSearch([FromQuery] GetFoodRecordSearchQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [EndpointDescription("Get food record by ID")]
    [ProducesResponseType(typeof(Result<FoodRecordDetailResponse?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetFoodRecordById(Guid id)
    {
        var result = await mediator.Send(new GetFoodRecordByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [EndpointDescription("Create a new food record")]
    [ProducesResponseType(typeof(Result<FoodRecordDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateFoodRecord([FromBody] CreateFoodRecordCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut]
    [EndpointDescription("Update an existing food record")]
    [ProducesResponseType(typeof(Result<FoodRecordDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateFoodRecord([FromBody] UpdateFoodRecordCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [EndpointDescription("Delete a food record by ID")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteFoodRecord(Guid id)
    {
        var result = await mediator.Send(new DeleteFoodRecordCommand([id]));
        return Ok(result);
    }

    [HttpDelete]
    [EndpointDescription("Delete multiple food records")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteFoodRecords([FromBody] DeleteFoodRecordCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
