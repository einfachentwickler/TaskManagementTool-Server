using System;

namespace TaskManagementTool.Common.Exceptions;

public class CustomException<TEnum>(TEnum errorCode, string message) : CustomExceptionBase(message) where TEnum : Enum
{
    public TEnum ErrorCodeTyped { get; } = errorCode;

    public override Enum ErrorCode => ErrorCodeTyped;
}