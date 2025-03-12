using Localizr.Core.ErrorCodes;

namespace Localizr.Projects.Abstractions.ErrorCodes;

public sealed class LocalizerProjectErrorCode(int code, string key) : LocalizrErrorCode
{

    public static LocalizerProjectErrorCode ProjectErrorCode => new(1000, nameof(ProjectErrorCode));
    public static LocalizerProjectErrorCode ProjectNotFound => new(1001, nameof(ProjectNotFound));
    public static LocalizerProjectErrorCode InvalidProjectName  => new(1002, nameof(InvalidProjectName));
    public static LocalizerProjectErrorCode InvalidProjectDescription => new(1003, nameof(InvalidProjectDescription));
    public static LocalizerProjectErrorCode InvalidDefaultLanguage => new(1004, nameof(InvalidDefaultLanguage));
    public static LocalizerProjectErrorCode InvalidSupportedLanguage => new(1005, nameof(InvalidSupportedLanguage));

    public override int Code { get; } = code;
    public override string Key { get; } = key;
    public override string ErrorNamespace => $"{base.ErrorNamespace}.Project";
}
