using System;

namespace Application.Services.Abstractions.DateTimeGeneration;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}