namespace Localizr.Core.ErrorCodes;

public abstract class SpreaViewErrorCode
{
    public abstract int Code { get; }
    public abstract string Key { get; }
    public virtual string TranslationKey => $"{ErrorNamespace}.{Key}";
    public virtual string ErrorNamespace => $"ErrorCode";
}