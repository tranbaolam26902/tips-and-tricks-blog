using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Data.Mappings {
    public class CommentMap : IEntityTypeConfiguration<Comment> {
        public void Configure(EntityTypeBuilder<Comment> builder) {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(c => c.PostedDate)
                .HasColumnType("datetime");
            builder.Property(c => c.IsApproved)
                .HasDefaultValue(false);
            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .HasConstraintName("FK_Comments_Posts")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
