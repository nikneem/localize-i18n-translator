using System.Text.Json;
using Localizr.Translations.Abstractions.Services;
using Localizr.Translations.Abstractions.ValueObjects;

namespace Localizr.Translations.Services;

public class TranslationsJsonFileParserService : IFileParserService
{
    public async Task<List<TranslationRootNode>> ImportFromFile(Stream stream, CancellationToken cancellationToken)
    {
        var reader = new StreamReader(stream);
        string jsonContent = await reader.ReadToEndAsync(cancellationToken);
        var translations = ParseJson(jsonContent);

        return translations;
    }

    public async Task<List<TranslationRootNode>> ImportFromFile(string filename, CancellationToken cancellationToken)
    {
        if (!File.Exists(filename))
        {
            throw new FileNotFoundException($"The file {filename} was not found.");
        }

        string jsonContent = await File.ReadAllTextAsync(filename, cancellationToken);
        var translations = ParseJson(jsonContent);
        return translations;
    }

    private List<TranslationRootNode> ParseJson(string jsonContent)
    {
        var translations = new List<TranslationRootNode>();
        var translationsAlternative = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var parentKey = string.Empty;

        foreach (var translationRootNode in translationsAlternative)
        {
            var translationNode = ParseNode(translationRootNode.Key, parentKey, translationRootNode.Value);
            translations.Add(
                new TranslationRootNode(
                    "en", 
                    true, 
                    translationNode.Key,
                    translationNode.FullNodeKey,
                    translationNode.Value,
                    translationNode.Children,
                    translationNode.IsChecked,
                    translationNode.CreatedOn,
                    translationNode.LastModifiedOn));
        }

        return translations;

    }

    private List<TranslationNode> GetChildNodes(string parentKey, JsonElement childNodes)
    {
        var deserializedChildNodes = JsonSerializer.Deserialize<Dictionary<string, object>>(
            childNodes.ToString(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var nodes = new List<TranslationNode>();
        foreach (var kvp in deserializedChildNodes)
        {
            var translationNode = ParseNode(kvp.Key, parentKey, kvp.Value);
            nodes.Add(translationNode);
        }

        return nodes;
    }

    private TranslationNode ParseNode(string key, string? parentKey, object childNode)
    {
        parentKey = string.IsNullOrWhiteSpace(parentKey) ? key : $"{parentKey}.{key}";
        string? value = null;
        List<TranslationNode>? children = null;
        if (childNode is JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                value = jsonElement.GetString();
            }

            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                children = GetChildNodes(parentKey, jsonElement);
            }
        }

        return new TranslationNode(key, parentKey, value, children, false, DateTimeOffset.UtcNow, null);
    }

    public async Task<bool> ExportToFile(
        string filename,
        List<TranslationRootNode> translations,
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
            WriteNode(writer, node);
        }
        writer.WriteEndObject();
        await writer.FlushAsync(cancellationToken);
        
        return true;
    }

    private void WriteNode(Utf8JsonWriter writer, TranslationNode node)
    {
        if (node.Value != null)
        {
            writer.WriteString(node.Key, node.Value);
        }
        else if (node.Children != null)
        {
            writer.WriteStartObject(node.Key);
            foreach (var child in node.Children)
            {
                WriteNode(writer, child);
            }
            writer.WriteEndObject();
        }
    }
}