using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(x => x.UserEmail).IsRequired();
        builder.Property(x => x.UserAgent).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.TokenHash).IsRequired();
    }
}