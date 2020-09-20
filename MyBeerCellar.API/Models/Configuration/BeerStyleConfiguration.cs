using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBeerCellar.API.Models.Configuration
{
    public class BeerStyleConfiguration : IEntityTypeConfiguration<BeerStyle>
    {
        public void Configure(EntityTypeBuilder<BeerStyle> builder)
        {
            builder.ToTable(nameof(BeerStyle), Constants.DataConfiguration.SchemaName);

            builder.HasKey(_ => _.StyleId);

            builder.Property(_ => _.StyleName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(_ => _.DateCreated)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.Property(_ => _.DateModified)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.HasData(new BeerStyle
            {
                StyleId = 1,
                StyleName = "American IPA"
            },
                new BeerStyle
                {
                    StyleId = 2,
                    StyleName = "New England IPA"
                },
                new BeerStyle
                {
                    StyleId = 3,
                    StyleName = "Imperial Stout"
                });
        }
    }
}
