using System;
using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.Common.Exceptions;

public class TaskManagementToolException : Exception
{
    public ApiErrorCode ErrorCode { get; set; }

    public string ErrorMessage { get; set; }

    public TaskManagementToolException() { }

    public TaskManagementToolException(string message, Exception innerException = null) : base(message, innerException) { }

    public TaskManagementToolException(ApiErrorCode errorCode, string message) : base(message)
    {
        ErrorMessage = message;
        ErrorCode = errorCode;
    }

    public TaskManagementToolException(string message) : base(message) { }
}