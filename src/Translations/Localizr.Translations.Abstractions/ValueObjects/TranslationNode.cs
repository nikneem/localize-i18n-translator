namespace Localizr.Translations.Abstractions.ValueObjects;

public record TranslationNode(
    string Key, 
    string FullNodeKey, 
    string? Value,
    List<TranslationNode>? Children,
    bool IsChecked,
    DateTimeOffset CreatedOn,
    DateTimeOffset? LastModifiedOn);
