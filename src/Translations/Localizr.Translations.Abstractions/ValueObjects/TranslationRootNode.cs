namespace Localizr.Translations.Abstractions.ValueObjects;

public record TranslationRootNode(
    string Key,
    string FullNodeKey,
    List<TranslationNode>? Children,
    List<TranslationValue> Values,
    bool IsChecked,
    DateTimeOffset CreatedOn,
    DateTimeOffset? LastModifiedOn)  : TranslationNode(Key, FullNodeKey, Children, Values,IsChecked, CreatedOn, LastModifiedOn)
{
    
}