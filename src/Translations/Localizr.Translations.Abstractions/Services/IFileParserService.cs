using Localizr.Translations.Abstractions.ValueObjects;

namespace Localizr.Translations.Abstractions.Services;

public interface IFileParserService
{
    Task<List<TranslationRootNode>> ImportFromFile(Stream stream, CancellationToken cancellationToken);
    Task<List<TranslationRootNode>> ImportFromFile(string filename, CancellationToken cancellationToken);
    Task<bool> ExportToFile(string filename, List<TranslationRootNode> translations, CancellationToken cancellationToken);
}