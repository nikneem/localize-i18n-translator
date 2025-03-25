namespace Localizr.Translations.DomainModels;

public class TranslationValue(string languageId, string? value)
{
    public string LanguageId { get; } = languageId;
    public string? Value { get; private set; } = value;

    public void SetValue(string? value)
    {
        if (!Equals(value, Value))
        {
            Value = value;
        }
    }
}