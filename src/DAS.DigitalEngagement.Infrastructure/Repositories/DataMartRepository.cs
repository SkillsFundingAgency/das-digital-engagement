using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Infrastructure.Repositories
{
    public class DataMartRepository : IDataMartRepository
    {
        private readonly string _connectionString;
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider;

        public DataMartRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.DataMart;
        }
        public DataMartRepository(IOptions<ConnectionStrings> connectionStrings, AzureServiceTokenProvider azureServiceTokenProvider)
        {
            _azureServiceTokenProvider = azureServiceTokenProvider;
            _connectionString = connectionStrings.Value.DataMart;
        }

        public IList<dynamic> RetrieveViewData(string viewName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (_azureServiceTokenProvider != null)
                {
                    connection.AccessToken = _azureServiceTokenProvider
                        .GetAccessTokenAsync("https://database.windows.net/").GetAwaiter().GetResult();

                }
                var data = connection.Query($"select * from {viewName}");
                return data.ToList();
            }
        }
    }
}
