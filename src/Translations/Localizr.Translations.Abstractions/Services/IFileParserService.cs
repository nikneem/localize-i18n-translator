using Localizr.Translations.Abstractions.ValueObjects;

namespace Localizr.Translations.Abstractions.Services;

public interface IFileParserService
{
    Task<List<TranslationRootNode>> ImportFromFile(Stream stream, string defaultLanguageId, CancellationToken cancellationToken);
    Task<List<TranslationRootNode>> ImportFromFile(string filename, string defaultLanguageId, CancellationToken cancellationToken);
    Task<bool> ExportToFile(
        string filename,
        List<TranslationRootNode> translations,
        string language,
        string defaultLanguage,
        CancellationToken cancellationToken);
}