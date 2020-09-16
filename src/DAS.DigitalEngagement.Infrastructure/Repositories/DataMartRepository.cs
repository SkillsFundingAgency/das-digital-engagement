using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;

namespace DAS.DigitalEngagement.Infrastructure.Repositories
{
    public class DataMartRepository : IDataMartRepository
    {
        private readonly string _connectionString;

        public DataMartRepository(IOptions<ConnectionStrings> connectionStrings)
        {
            _connectionString = connectionStrings.Value.DataMart;
        }

        public IList<dynamic> RetrieveViewData(string viewName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var data = connection.Query($"select * from {viewName}");
                return data.ToList();
            }
        }
    }
}
