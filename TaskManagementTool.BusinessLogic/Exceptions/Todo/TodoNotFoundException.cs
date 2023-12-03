using System;
using TaskManagementTool.BusinessLogic.Enums;

namespace TaskManagementTool.BusinessLogic.Exceptions.Todo;

[Serializable]
public class TodoNotFoundException : Exception
{
    public static ErrorCodes ErrorCode => ErrorCodes.TodoNotFound;

    public TodoNotFoundException()
    {
    }

    public TodoNotFoundException(string message) : base(message)
    {
    }

    public TodoNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}