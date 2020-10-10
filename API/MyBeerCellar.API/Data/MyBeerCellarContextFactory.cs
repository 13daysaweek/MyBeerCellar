using System;
using Microsoft.EntityFrameworkCore;

namespace MyBeerCellar.API.Data
{
    public class MyBeerCellarContextFactory : IMyBeerCellarContextFactory
    {
        private readonly string _connectionString;
        private readonly AadTokenInjectorDbInterceptor _aadTokenInjectorDbInterceptor;

        public MyBeerCellarContextFactory(string connectionString, AadTokenInjectorDbInterceptor aadTokenInjectorDbInterceptor)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            _connectionString = connectionString;

            _aadTokenInjectorDbInterceptor = aadTokenInjectorDbInterceptor ?? throw new ArgumentNullException(nameof(aadTokenInjectorDbInterceptor));
        }

        public MyBeerCellarContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyBeerCellarContext>();
            optionsBuilder.UseSqlServer(_connectionString)
                .AddInterceptors(_aadTokenInjectorDbInterceptor);

            return new MyBeerCellarContext(optionsBuilder.Options);
        }
    }
}