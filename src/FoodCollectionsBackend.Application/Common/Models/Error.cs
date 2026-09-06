using FoodCollectionsBackend.Application.Common.Enum;

namespace FoodCollectionsBackend.Application.Common.Models;

public class Error
{
    public ErrorCode Code { get; set; }
    public string? Message { get; set; }

    public static Error None
        => new()
        {
            Code = ErrorCode.None,
            Message = string.Empty
        };

    public static Error NotFound(string message)
        => new()
        {
            Code = ErrorCode.NotFound,
            Message = message
        };

    public static Error Invalid(string message)
        => new()
        {
            Code = ErrorCode.Invalid,
            Message = message
        };

    public static Error Unauthorized(string message)
        => new()
        {
            Code = ErrorCode.Unauthorized,
            Message = message
        };

    public static Error Conflict(string message)
        => new()
        {
            Code = ErrorCode.Conflict,
            Message = message
        };
}
