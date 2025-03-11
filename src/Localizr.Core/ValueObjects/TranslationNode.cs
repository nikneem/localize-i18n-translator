namespace Localizr.Core.ValueObjects;

public record TranslationNode(
    string? Value,
    string? TranslationValue,
    bool IsChecked,
    bool IsLocked,
    Dictionary<string, TranslationNode>? Children
    );