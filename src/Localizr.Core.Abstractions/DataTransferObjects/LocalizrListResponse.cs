namespace Localizr.Core.Abstractions.DataTransferObjects;

public record LocalizrListResponse<TObject>(
    bool IsSuccess,
    List<TObject> Data,
    int Page,
    int PageSize,
    int TotalPages,
    string? ErrorMessage = null)
{
    public static LocalizrListResponse<TObject> Success(
        List<TObject> result,
        int page = 0,
        int pageSize = 0,
        int totalPages = 0) =>
        new(true, result, page, pageSize, totalPages);

    public static LocalizrListResponse<TObject> Fail(string errorMessage) =>
        new(false, null!, 0, 0, 0, errorMessage);
}