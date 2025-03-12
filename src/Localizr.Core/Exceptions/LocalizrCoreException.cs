using Localizr.Core.ErrorCodes;

namespace Localizr.Core.Exceptions;

public class LocalizrCoreException(
    LocalizrCoreErrorCode errorCode,
    string message,
    Exception? innerException = null) : LocalizrException(errorCode, message, innerException);