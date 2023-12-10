using System;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.Common.Exceptions;

[Serializable]
public class UserNotFoundExpection : TaskManagementToolException
{
    public static ApiErrorCodes ErrorCode => ApiErrorCodes.UserNotFound;

    public UserNotFoundExpection() { }

    public UserNotFoundExpection(string message, Exception innerException = null) : base(message, innerException) { }

    public UserNotFoundExpection(string message) : base(message) { }
}
