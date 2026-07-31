namespace HolidayShowdown.Models;

public class OperationResult<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T? Data { get; }

    private OperationResult(bool success, string message, T? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static OperationResult<T> Ok(T data, string message = "OK")
        => new(true, message, data);

    public static OperationResult<T> Fail(string message)
        => new(false, message, default);
}