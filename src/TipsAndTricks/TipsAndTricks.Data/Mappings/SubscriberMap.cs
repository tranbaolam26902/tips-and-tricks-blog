using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Data.Mappings {
    public class SubscriberMap : IEntityTypeConfiguration<Subscriber> {
        public void Configure(EntityTypeBuilder<Subscriber> builder) {
            builder.ToTable("Subscribers");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(s => s.SubscribedDate)
                .HasColumnType("datetime");
            builder.Property(s => s.UnsubscribedDate)
                .HasColumnType("datetime");
            builder.Property(s => s.SubscribeState)
                .IsRequired();
            builder.Property(s => s.PreviousBannedState);
            builder.Property(s => s.Reason)
                .HasMaxLength(500);
            builder.Property(s => s.Notes)
                .HasMaxLength(500);
        }
    }
}
