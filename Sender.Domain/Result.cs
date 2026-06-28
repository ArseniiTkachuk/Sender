public record Unit();

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    public int StatusCode { get; }

    private Result(bool success, T value, string error, int statusCode)
    {
        IsSuccess = success;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value, int statusCode = 200) => new(true, value, null, statusCode);

    public static Result<Unit> Success() => new(true, new Unit(), null, 204);

    public static Result<T> Failure(string error, int statusCode = 400) => new(false, default, error, statusCode);
}