namespace Localizr.Translations.Abstractions.ValueObjects;

public record TranslationRootNode(
    string LanguageKey, 
    bool IsDefault,
    string Key,
    string FullNodeKey,
    string? Value,
    List<TranslationNode>? Children,
    bool IsChecked,
    DateTimeOffset CreatedOn,
    DateTimeOffset? LastModifiedOn)  : TranslationNode(Key, FullNodeKey, Value, Children, IsChecked, CreatedOn, LastModifiedOn)
{
    
}