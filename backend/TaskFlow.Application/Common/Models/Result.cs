namespace TaskFlow.Application.Common.Models;

/// <summary>
/// Generic Result pattern - API response'ları için standart format
/// İşlem başarılı mı/başarısız mı bilgisini ve data/error döndürür
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public List<string> Errors { get; private set; } = new();

    private Result(bool isSuccess, T? data, string? errorMessage, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        Errors = errors ?? new List<string>();
    }

    // Başarılı sonuç döndür
    public static Result<T> Success(T data)
        => new(true, data, null);

    // Tek hata mesajı ile başarısız sonuç
    public static Result<T> Failure(string errorMessage)
        => new(false, default, errorMessage);

    // Birden fazla hata ile başarısız sonuç (validation errors)
    public static Result<T> Failure(List<string> errors)
        => new(false, default, null, errors);
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public List<string> Errors { get; private set; } = new();

    protected Result(bool isSuccess, string? errorMessage, List<string>? errors = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Errors = errors ?? new List<string>();
    }

    public static Result Success() => new(true, null);

    public static Result Failure(string errorMessage) => new(false, errorMessage);

    public static Result Failure(List<string> errors) => new(false, null, errors);
}