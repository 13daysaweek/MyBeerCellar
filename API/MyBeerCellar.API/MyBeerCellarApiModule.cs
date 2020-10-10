using System;
using Autofac;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using MyBeerCellar.API.Data;

namespace MyBeerCellar.API
{
    public class MyBeerCellarApiModule : Module
    {
        private readonly IConfiguration _configuration;

        public MyBeerCellarApiModule(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override void Load(ContainerBuilder builder)
        {
            var sqlConnectionString = _configuration[Constants.ConfigurationKeys.MyBeerCellarDbConnectionString];

            builder.RegisterType<AadTokenInjectorDbInterceptor>()
                .WithParameter("azureServiceTokenProvider", new AzureServiceTokenProvider());

            builder.RegisterType<MyBeerCellarContextFactory>()
                .AsImplementedInterfaces()
                .WithParameter("connectionString", sqlConnectionString);
        }
    }
}