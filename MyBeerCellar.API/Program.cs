using System;
using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyBeerCellar.API.Data;

namespace MyBeerCellar.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args != null && args.Any(_ => _.ToLower() == "run-migration"))
            {
                RunMigration();

                return;
            }
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(_ => _.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();
                    config.AddAzureAppConfiguration(settings[Constants.ConfigurationKeys.AppConfigConnectionStringKey]);
                })
                .UseStartup<Startup>());

        private static void RunMigration()
        {
            var db = new MyBeerCellarContext();
            Console.WriteLine("Getting ready to run the migration");
            db.Database.Migrate();
            Console.WriteLine("Done!");
        }
    }
}
