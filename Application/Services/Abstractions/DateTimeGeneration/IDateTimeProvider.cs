using System;

namespace Application.Services.Abstractions.DateTimeGeneration;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}