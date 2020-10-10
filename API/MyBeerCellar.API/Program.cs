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
            if (args != null && args.Any(_ => _.ToLower() == Constants.MigrationUtility.LaunchPrefix))
            {
                RunMigration(args);
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

        private static void RunMigration(string[] args)
        {
            if (args.Any(_ => _.ToLower().StartsWith(Constants.MigrationUtility.ConnectionStringArg)))
            {
                var arg = args.First(_ => _.ToLower().StartsWith(Constants.MigrationUtility.ConnectionStringArg));
                var connectionString = arg.Replace("--connection-string=", string.Empty);

                var context = new MyBeerCellarContext(connectionString);

                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Applying migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("Migrations applied successfully");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exception caught while applying migrations:");
                    Console.WriteLine();
                    Console.WriteLine(e);
                    Console.ResetColor();
                    throw;
                }
            }
        }
    }
}
