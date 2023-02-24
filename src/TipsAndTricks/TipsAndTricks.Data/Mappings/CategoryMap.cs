using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Data.Mappings {
    public class CategoryMap : IEntityTypeConfiguration<Category> {
        public void Configure(EntityTypeBuilder<Category> builder) {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.Description)
                .HasMaxLength(500);
            builder.Property(c => c.UrlSlug)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(c => c.ShowOnMenu)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
