using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration;

public class ToDoEntityConfiguration : IEntityTypeConfiguration<ToDoEntity>
{
    public void Configure(EntityTypeBuilder<ToDoEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasOne(t => t.Creator)
               .WithMany()
               .HasForeignKey(t => t.CreatorId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Content).IsRequired();
    }
}