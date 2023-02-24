﻿using Microsoft.EntityFrameworkCore;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.Data.Mappings {
    public class AuthorMap : IEntityTypeConfiguration<Author> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Author> builder) {
            builder.ToTable("Authors");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.UrlSlug)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.ImageUrl)
                .HasMaxLength(500);
            builder.Property(a => a.Email)
                .HasMaxLength(150);
            builder.Property(a => a.JoinedDate)
                .HasColumnType("datetime");
            builder.Property(a => a.Notes)
                .HasMaxLength(500);
        }
    }
}
