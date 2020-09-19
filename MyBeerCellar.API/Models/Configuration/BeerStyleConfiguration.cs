using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBeerCellar.API.Models.Configuration
{
    public class BeerStyleConfiguration : IEntityTypeConfiguration<BeerStyle>
    {
        public void Configure(EntityTypeBuilder<BeerStyle> builder)
        {
            builder.HasKey(_ => _.StyleId);

            builder.Property(_ => _.StyleName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType(Constants.DataConfiguration.NvarcharDataType);

            builder.Property(_ => _.DateCreated)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.Property(_ => _.DateModified)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);
        }
    }
}
