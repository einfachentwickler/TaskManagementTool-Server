using System;

namespace TaskManagementTool.Common.Exceptions;

public class TaskManagementToolException : Exception
{
    public TaskManagementToolException() { }

    public TaskManagementToolException(string message, Exception innerException = null) : base(message, innerException) { }

    public TaskManagementToolException(string message) : base(message) { }
}