using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBeerCellar.API.Models.Configuration
{
    public class CellarItemConfiguration : IEntityTypeConfiguration<CellarItem>
    {
        public void Configure(EntityTypeBuilder<CellarItem> builder)
        {
            builder.ToTable(nameof(CellarItem), Constants.DataConfiguration.SchemaName);

            builder.HasKey(_ => _.CellarItemId);

            builder.Property(_ => _.ItemName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(_ => _.YearProduced)
                .IsRequired();

            builder.Property(_ => _.Quantity)
                .IsRequired();

            builder.Property(_ => _.DateCreated)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.Property(_ => _.DateModified)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.HasIndex(_ => new {_.ItemName, _.YearProduced})
                .IsUnique();
        }
    }
}