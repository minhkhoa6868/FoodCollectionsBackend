namespace FoodCollectionsBackend.Application.Common.Models;

public class Error
{
    public string? Code { get; set; }
    public string? Message { get; set; }

    public static Error None => new();

    public static Error NotFound(string message)
        => new()
        {
            Code = "NotFound",
            Message = message
        };

    public static Error Validation(string message)
        => new()
        {
            Code = "Validation",
            Message = message
        };

    public static Error Unauthorized(string message)
        => new()
        {
            Code = "Unauthorized",
            Message = message
        };

    public static Error Conflict(string message)
        => new()
        {
            Code = "Conflict",
            Message = message
        };
}
