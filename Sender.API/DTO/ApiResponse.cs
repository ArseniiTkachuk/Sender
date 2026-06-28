public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string message = "Успішно")
    {
        return new ApiResponse<T> { Success = true, Data = data, Message = message };
    }

    public static ApiResponse<T> FailureResult(string message)
    {
        return new ApiResponse<T> { Success = false, Message = message, Data = default };
    }
}