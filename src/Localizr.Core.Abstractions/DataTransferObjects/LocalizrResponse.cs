namespace Localizr.Core.Abstractions.DataTransferObjects;

public record LocalizrResponse<TObject>(bool IsSuccess, TObject Data, string? ErrorMessage = null)
{
    public static LocalizrResponse<TObject> Success(TObject result) => new(true, result);
    public static LocalizrResponse<TObject> Fail(string errorMessage) => new(false, default!, errorMessage);
}