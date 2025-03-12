using Localizr.Core.Exceptions;
using Localizr.Projects.Abstractions.ErrorCodes;

namespace Localizr.Projects.Abstractions.Exceptions;

public class LocalizrProjectException(
    LocalizerProjectErrorCode errorCode,
    string message,
    Exception? innerException = null) : LocalizrException(errorCode, message, innerException);