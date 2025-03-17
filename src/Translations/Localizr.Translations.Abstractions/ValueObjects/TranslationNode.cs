namespace Localizr.Translations.Abstractions.ValueObjects;

public class TranslationNode
{
    public string? Value { get; set; }
    public Dictionary<string, TranslationNode>? Children { get; set; } = new();
}