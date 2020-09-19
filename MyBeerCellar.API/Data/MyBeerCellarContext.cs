using Microsoft.EntityFrameworkCore;

namespace MyBeerCellar.API.Data
{
    public class MyBeerCellarContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
