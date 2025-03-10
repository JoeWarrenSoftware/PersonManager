namespace UKParliament.CodeTest.Services.Helpers;

/// <summary>
/// Non-data version
/// </summary>
public class ServiceResult
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    private ServiceResult(bool success, string? errorMessage)
    {
        IsSuccess = success;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult Success() => new ServiceResult(true, null);
    public static ServiceResult Failure(string message) => new ServiceResult(false, message);
}

/// <summary>
/// Data version
/// </summary>
/// <typeparam name="T">Data to be returned with the service result</typeparam>
public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }

    private ServiceResult(bool success, T? data, string? errorMessage)
    {
        IsSuccess = success;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> Success(T? data) => new ServiceResult<T>(true, data, null);
    public static ServiceResult<T> Failure(string message) => new ServiceResult<T>(false, default, message);
}