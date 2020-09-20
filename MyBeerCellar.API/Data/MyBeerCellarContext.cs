using Microsoft.EntityFrameworkCore;

namespace MyBeerCellar.API.Data
{
    public class MyBeerCellarContext : DbContext
    {
        public MyBeerCellarContext()
        {
            
        }

        public MyBeerCellarContext(DbContextOptions<MyBeerCellarContext> optionsBuilderOptions) : base(optionsBuilderOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
