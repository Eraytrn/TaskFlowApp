using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Persistence.Configurations;

public class BoardJoinRequestConfiguration : IEntityTypeConfiguration<BoardJoinRequest>
{
    public void Configure(EntityTypeBuilder<BoardJoinRequest> builder)
    {
        builder.ToTable("BoardJoinRequests");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Status)
            .IsRequired();

        builder.Property(r => r.RequestDate)
            .IsRequired();

        // Relationships
        builder.HasOne(r => r.Board)
            .WithMany(b => b.JoinRequests)
            .HasForeignKey(r => r.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Requester)
            .WithMany()
            .HasForeignKey(r => r.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Responder)
            .WithMany()
            .HasForeignKey(r => r.ResponderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index for faster queries
        builder.HasIndex(r => new { r.BoardId, r.RequesterId, r.Status });
    }
}
