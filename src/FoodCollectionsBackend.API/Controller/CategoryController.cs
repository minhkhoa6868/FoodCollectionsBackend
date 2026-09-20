using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using FoodCollectionsBackend.Application.Common.Models;
using FoodCollectionsBackend.Application.Features.Categories.Models;
using FoodCollectionsBackend.Application.Features.Categories.Queries;
using FoodCollectionsBackend.Application.Features.Categories.Commands;
namespace FoodCollectionsBackend.API.Controller;

[ApiController]
[Authorize]
[Route("api/categories")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get a list of categories with search and pagination.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [EndpointDescription("Get category search with pagination")]
    [ProducesResponseType(typeof(Result<PaginationResponse<CategoryResponse>?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCategorySearch([FromQuery] GetCategorySearchQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get category by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [EndpointDescription("Get category by id")]
    [ProducesResponseType(typeof(Result<CategoryResponse?>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [EndpointDescription("Create a new category")]
    [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut]
    [EndpointDescription("Update an existing category")]
    [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Delete a category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [EndpointDescription("Delete a category by id")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var result = await mediator.Send(new DeleteCategoryCommand([id]));
        return Ok(result);
    }

    /// <summary>
    /// Delete multiple categories by ids
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpDelete]
    [EndpointDescription("Delete multiple categories by ids")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategories([FromBody] DeleteCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
