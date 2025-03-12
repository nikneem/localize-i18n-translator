using Localizr.Core.ErrorCodes;

namespace Localizr.Core.Exceptions;

public class LocalizrException(LocalizrErrorCode errorCode, string message, Exception? innerException = null) : Exception(message, innerException)
{
    public LocalizrErrorCode ErrorCode => errorCode;

}