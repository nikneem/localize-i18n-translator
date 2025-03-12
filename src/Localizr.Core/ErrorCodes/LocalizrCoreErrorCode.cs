namespace Localizr.Core.ErrorCodes;

public sealed class LocalizrCoreErrorCode(int code, string key) : LocalizrErrorCode
{

    public static LocalizrCoreErrorCode LanguageNotFound => new(1000, nameof(LanguageNotFound));


    public override int Code { get; } = code;
    public override string Key { get; } = key;
    public override string ErrorNamespace => $"{base.ErrorNamespace}.Core";
}
