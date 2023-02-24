using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Data.Mappings {
    public class TagMap : IEntityTypeConfiguration<Tag> {
        public void Configure(EntityTypeBuilder<Tag> builder) {
            builder.ToTable("Tags");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .HasMaxLength(500);
            builder.Property(t => t.UrlSlug)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
