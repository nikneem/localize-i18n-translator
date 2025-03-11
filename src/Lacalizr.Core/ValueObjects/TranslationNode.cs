namespace Lacalizr.Core.ValueObjects;

public record TranslationNode(string? Value, Dictionary<string, TranslationNode>? Children);