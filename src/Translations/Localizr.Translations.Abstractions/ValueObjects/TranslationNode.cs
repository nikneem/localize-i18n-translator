namespace Localizr.Translations.Abstractions.ValueObjects;

public record TranslationNode(
    string Key, 
    string FullNodeKey, 
    List<TranslationNode>? Children,
    List<TranslationValue> Values,
bool IsChecked,
    DateTimeOffset CreatedOn,
    DateTimeOffset? LastModifiedOn);
