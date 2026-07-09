using FoodCollectionsBackend.Application.Common.Exceptions;
namespace FoodCollectionsBackend.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning(
                ex,
                "Bad request occured. Path: {Path}",
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = ex.Message,
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(
                ex,
                "Unauthorized access. Path: {Path}",
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = ex.Message,
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(
                ex,
                "Not Found occured. Path: {Path}",
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = ex.Message,
            });
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(
                ex,
                "Conflict occured. Path: {Path}",
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status409Conflict;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = ex.Message,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred. Path: {Path}",
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
}
