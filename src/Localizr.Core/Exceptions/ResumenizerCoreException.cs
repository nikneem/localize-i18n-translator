using Localizr.Core.ErrorCodes;

namespace Localizr.Core.Exceptions;

public class ResumenizerCoreException(
    ResumenizerCoreErrorCode errorCode,
    string message,
    Exception? innerException = null) : ResumenizerException(errorCode, message, innerException);