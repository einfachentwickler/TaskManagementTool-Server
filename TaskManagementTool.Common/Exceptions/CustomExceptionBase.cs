using System;

namespace TaskManagementTool.Common.Exceptions;

public abstract class CustomExceptionBase(string message) : Exception(message)
{
    public abstract Enum ErrorCode { get; }
}