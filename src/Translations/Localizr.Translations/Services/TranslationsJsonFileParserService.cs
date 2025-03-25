using System.Text.Json;
using Localizr.Translations.Abstractions.Services;
using Localizr.Translations.Abstractions.ValueObjects;

namespace Localizr.Translations.Services;

public class TranslationsJsonFileParserService : IFileParserService
{
    public async Task<List<TranslationRootNode>> ImportFromFile(Stream stream, string defaultLanguageId, CancellationToken cancellationToken)
    {
        var reader = new StreamReader(stream);
        string jsonContent = await reader.ReadToEndAsync(cancellationToken);
        var translations = ParseJson(jsonContent, defaultLanguageId);

        return translations;
    }

    public async Task<List<TranslationRootNode>> ImportFromFile(string filename, string defaultLanguageId, CancellationToken cancellationToken)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException($"The file {filename} was not found.");
        }

        string jsonContent = await File.ReadAllTextAsync(filename, cancellationToken);
        var translations = ParseJson(jsonContent, defaultLanguageId);
        return translations;
    }

    private List<TranslationRootNode> ParseJson(string jsonContent, string defaultLanguageId)
    {
        var translations = new List<TranslationRootNode>();
        var translationsAlternative = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var parentKey = string.Empty;

        foreach (var translationRootNode in translationsAlternative)
        {
            var translationNode = ParseNode(translationRootNode.Key, parentKey, defaultLanguageId, translationRootNode.Value);
            translations.Add(
                new TranslationRootNode(
                    translationNode.Key,
                    translationNode.FullNodeKey,
                    translationNode.Children,
                    translationNode.Values,
                    translationNode.IsChecked,
                    translationNode.CreatedOn,
                    translationNode.LastModifiedOn));
        }

        return translations;

    }

    private List<TranslationNode> GetChildNodes(string parentKey, string languageId, JsonElement childNodes)
    {
        var deserializedChildNodes = JsonSerializer.Deserialize<Dictionary<string, object>>(
            childNodes.ToString(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var nodes = new List<TranslationNode>();
        foreach (var kvp in deserializedChildNodes)
        {
            var translationNode = ParseNode(kvp.Key, parentKey, languageId, kvp.Value);
            nodes.Add(translationNode);
        }

        return nodes;
    }

    private TranslationNode ParseNode(string key, string? parentKey, string languageId, object childNode)
    {
        parentKey = string.IsNullOrWhiteSpace(parentKey) ? key : $"{parentKey}.{key}";
        var values = new List<TranslationValue>();
        List<TranslationNode>? children = null;
        if (childNode is JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                var value = jsonElement.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    values.Add(new TranslationValue(languageId, value));
                }
            }

            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                children = GetChildNodes(parentKey, languageId, jsonElement);
            }
        }

        return new TranslationNode(key, parentKey, children, values, false, DateTimeOffset.UtcNow, null);
    }

    public async Task<bool> ExportToFile(
        string filename,
        List<TranslationRootNode> translations, 
        string language, 
        string defaultLanguage,
        CancellationToken cancellationToken)
    {

        var file = new FileInfo(filename);
        if (file.Exists)
        {
            file.Delete();
        }

        await using var fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        var jsonWriterOptions = new JsonWriterOptions { Indented = true };
        var writer = new Utf8JsonWriter(fileStream, jsonWriterOptions);
        writer.WriteStartObject();
        foreach (var node in translations)
        {
            WriteNode(writer, node, language, defaultLanguage);
        }
        writer.WriteEndObject();
        await writer.FlushAsync(cancellationToken);
        
        return true;
    }

    private void WriteNode(Utf8JsonWriter writer, TranslationNode node, string language, string defaultLanguage)
    {
        var value = node.Values.FirstOrDefault(v => v.LanguageId == language) ??
                    node.Values.FirstOrDefault(v => v.LanguageId == defaultLanguage);

        if (value != null)
        {
            writer.WriteString(node.Key, value.Value);
        }
        else if (node.Children != null)
        {
            writer.WriteStartObject(node.Key);
            foreach (var child in node.Children)
            {
                WriteNode(writer, child,language, defaultLanguage);
            }
            writer.WriteEndObject();
        }
    }
}