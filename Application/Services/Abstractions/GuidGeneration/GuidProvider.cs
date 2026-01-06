using System;

namespace Application.Services.Abstractions.GuidGeneration;

public class GuidProvider : IGuidProvider
{
    public Guid NewGuid => Guid.NewGuid();
}