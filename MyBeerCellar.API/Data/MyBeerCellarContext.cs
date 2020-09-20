using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Data
{
    public class MyBeerCellarContext : DbContext
    {
        public MyBeerCellarContext()
        {
            
        }

        public MyBeerCellarContext(DbContextOptions<MyBeerCellarContext> optionsBuilderOptions) : base(optionsBuilderOptions)
        {
            var conn = (Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection();
            conn.AccessToken = (new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
        }

        public DbSet<BeerStyle> BeerStyles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
