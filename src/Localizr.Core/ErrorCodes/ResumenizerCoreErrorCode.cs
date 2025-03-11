namespace Localizr.Core.ErrorCodes;

public sealed class ResumenizerCoreErrorCode(int code, string key) : SpreaViewErrorCode
{

    public static ResumenizerCoreErrorCode LanguageNotFound => new(1000, nameof(LanguageNotFound));


    public override int Code { get; } = code;
    public override string Key { get; } = key;
    public override string ErrorNamespace => $"{base.ErrorNamespace}.Core";
}
