using System;

namespace Infrastructure.Entities;

public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public string TokenHash { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public required string CreatedByIp { get; init; }
    public string RevokedByIp { get; init; }
    public required string UserAgent { get; init; }
}