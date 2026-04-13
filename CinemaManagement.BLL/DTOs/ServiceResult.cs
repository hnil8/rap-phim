namespace CinemaManagement.BLL.DTOs;

/// <summary>
/// Kết quả trả về chuẩn từ tầng BLL — dùng chung toàn hệ thống.
/// </summary>
public class ServiceResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public static ServiceResult Success(string message = "Thành công", object? data = null)
        => new() { IsSuccess = true, Message = message, Data = data };

    public static ServiceResult Fail(string message, object? data = null)
        => new() { IsSuccess = false, Message = message, Data = data };
}

/// <summary>
/// ServiceResult có generic type cho Data. 
/// </summary>
public class ServiceResult<T> : ServiceResult
{
    public new T? Data { get; set; }

    public static ServiceResult<T> Success(T data, string message = "Thành công")
        => new() { IsSuccess = true, Message = message, Data = data };

    public static ServiceResult<T> Fail(string message)
        => new() { IsSuccess = false, Message = message };
}
