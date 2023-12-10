using System;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.Common.Exceptions;

[Serializable]
public class TodoNotFoundException : TaskManagementToolException
{
    public static ApiErrorCodes ErrorCode => ApiErrorCodes.TodoNotFound;

    public TodoNotFoundException() { }

    public TodoNotFoundException(string message, Exception innerException = null) : base(message, innerException) { }

    public TodoNotFoundException(string message) : base(message) { }
}