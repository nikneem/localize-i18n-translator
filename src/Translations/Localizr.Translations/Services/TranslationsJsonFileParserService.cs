using Localizr.Translations.DomainModels;
using System.Text.Json;

namespace Localizr.Translations.Services;

public class TranslationsJsonFileParserService
{
    public async Task ImportFromJsonAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file {filePath} was not found.");
        }

        string jsonContent = File.ReadAllText(filePath);
        _translations = JsonSerializer.Deserialize<Dictionary<string, TranslationNode>>(jsonContent)
                        ?? new Dictionary<string, TranslationNode>();

        foreach (var kvp in _translations)
        {
            await _container.UpsertItemAsync(new { id = kvp.Key, data = kvp.Value });
        }
    }
}