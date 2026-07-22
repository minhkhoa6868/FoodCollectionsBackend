namespace FoodCollectionsBackend.Application.Common.Models;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public Error? Error { get; set; }

    private Result() { }

    public static Result<T> SuccessResult(T data)
    {
        return new Result<T>
        {
            Success = true,
            Data = data
        };
    }

    public static Result<T> FailResult(Error error)
    {
        return new Result<T>
        {
            Success = false,
            Error = error
        };
    }
}

public class Result
{
    public bool Success { get; set; }
    public Error? Error { get; set; }

    private Result() { }

    public static Result SuccessResult()
    {
        return new Result
        {
            Success = true
        };
    }

    public static Result FailResult(Error error)
    {
        return new Result
        {
            Success = false,
            Error = error
        };
    }
}
