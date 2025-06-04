namespace lib.Services;

public class ServiceResult
{
    public const string SomethingWentWrong = "Что то пошло не так!";
    protected const string _successStatus = "Success";
    protected const string _errorStatus = "Error";

    public bool IsSuccess { get; set; }
    public string Status { get; set; } = null!;
    public string? Message { get; set; }

    public static ServiceResult Ok(string? message = null) =>
           new()
           { IsSuccess = true, Status = _successStatus, Message = message };

    public static ServiceResult Fail(string? message = null) =>
        new()
        { IsSuccess = false, Status = _errorStatus, Message = message };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> Ok(T? data, string? message)
    {
        return new ServiceResult<T> { IsSuccess = true, Status = _successStatus, Data = data, Message = message };
    }

    public static new ServiceResult<T> Ok(string? message)
    {
        return new ServiceResult<T> { IsSuccess = true, Status = _successStatus, Message = message };
    }

    public static new ServiceResult<T> Fail(string? message)
    {
        return new ServiceResult<T> { IsSuccess = false, Status = _errorStatus, Message = message };
    }

    public static ServiceResult<T> Fail(T? data, string? message)
    {
        return new ServiceResult<T> { IsSuccess = false, Status = _errorStatus, Data = data, Message = message };
    }
}
