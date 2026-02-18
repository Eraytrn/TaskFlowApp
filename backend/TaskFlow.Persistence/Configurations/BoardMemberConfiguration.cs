using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Persistence.Configurations;

public class BoardMemberConfiguration : IEntityTypeConfiguration<BoardMember>
{
    public void Configure(EntityTypeBuilder<BoardMember> builder)
    {
        builder.ToTable("BoardMembers");

        builder.HasKey(bm => bm.Id);

        builder.Property(bm => bm.Role)
            .IsRequired();

        builder.Property(bm => bm.JoinedDate)
            .IsRequired();

        // Relationships
        builder.HasOne(bm => bm.Board)
            .WithMany(b => b.Members)
            .HasForeignKey(bm => bm.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bm => bm.User)
            .WithMany(u => u.BoardMemberships)
            .HasForeignKey(bm => bm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: One user can only have one membership per board
        builder.HasIndex(bm => new { bm.BoardId, bm.UserId })
            .IsUnique();
    }
}
