using System;

namespace Application.Services.Abstractions.GuidGeneration;

public interface IGuidProvider
{
    Guid NewGuid { get; }
}