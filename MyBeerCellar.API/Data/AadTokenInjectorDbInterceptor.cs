using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MyBeerCellar.API.Data
{
    public class AadTokenInjectorDbInterceptor : DbConnectionInterceptor
    {
        private const string SCOPE = "https://database.windows.net";
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider;

        public AadTokenInjectorDbInterceptor(AzureServiceTokenProvider azureServiceTokenProvider)
        {
            _azureServiceTokenProvider = azureServiceTokenProvider;
        }

        public override async Task<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = new CancellationToken())
        {
            if (connection is SqlConnection sqlConnection)
            {
                sqlConnection.AccessToken = await GetAccessTokenAsync();
            }

            return result;
        }

        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            if (connection is SqlConnection sqlConnection)
            {
                sqlConnection.AccessToken = GetAccessTokenAsync().Result;
            }

            return result;
        }

        private Task<string> GetAccessTokenAsync() => _azureServiceTokenProvider.GetAccessTokenAsync(SCOPE);
    }
}
