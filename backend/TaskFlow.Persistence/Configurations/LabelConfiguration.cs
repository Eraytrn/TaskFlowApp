using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Persistence.Configurations;

public class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.ToTable("Labels");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.ColorHex)
            .IsRequired()
            .HasMaxLength(7);

        // One Board many Labels
        builder.HasOne(l => l.Board)
            .WithMany(b => b.Labels)
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
