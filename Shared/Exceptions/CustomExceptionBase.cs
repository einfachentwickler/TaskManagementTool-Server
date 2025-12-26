using System;

namespace Shared.Exceptions;

public abstract class CustomExceptionBase(string message) : Exception(message)
{
    public abstract Enum ErrorCode { get; }
}