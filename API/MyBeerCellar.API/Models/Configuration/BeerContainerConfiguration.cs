using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyBeerCellar.API.Models.Configuration
{
    public class BeerContainerConfiguration : IEntityTypeConfiguration<BeerContainer>
    {
        public void Configure(EntityTypeBuilder<BeerContainer> builder)
        {
            builder.ToTable(nameof(BeerContainer), Constants.DataConfiguration.SchemaName);

            builder.HasKey(_ => _.BeerContainerId);

            builder.Property(_ => _.ContainerType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(_ => _.DateCreated)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.Property(_ => _.DateModified)
                .IsRequired()
                .HasDefaultValueSql(Constants.DataConfiguration.CurrentUtcDateTimeDefault);

            builder.HasIndex(_ => _.ContainerType)
                .IsUnique();

            builder.HasData(new BeerContainer
                {
                    BeerContainerId = 1,
                    ContainerType = "Can"
                },
                new BeerContainer
                {
                    BeerContainerId = 2,
                    ContainerType = "Bottle"
                },
                new BeerContainer
                {
                    BeerContainerId = 3,
                    ContainerType = "Growler"
                });
        }
    }
}