﻿using System;
using Microsoft.EntityFrameworkCore;
using MyBeerCellar.API.Models;

namespace MyBeerCellar.API.Data
{
    public class MyBeerCellarContext : DbContext
    {
        private string _connectionString;

        public MyBeerCellarContext()
        {
            
        }

        public MyBeerCellarContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public MyBeerCellarContext(DbContextOptions<MyBeerCellarContext> optionsBuilderOptions) : base(optionsBuilderOptions)
        {
            if (!Database.ProviderName.Contains("Memory"))
            {
                var conn = (Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection();
                conn.AccessToken = (new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
            }
        }

        public DbSet<BeerStyle> BeerStyles { get; set; }

        public DbSet<BeerContainer> BeerContainers { get; set; }

        public DbSet<CellarItem> CellarItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    var envVar = Environment.GetEnvironmentVariable("MIGRATION_CONNECTION_STRING");
                    _connectionString = envVar;
                }

                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
