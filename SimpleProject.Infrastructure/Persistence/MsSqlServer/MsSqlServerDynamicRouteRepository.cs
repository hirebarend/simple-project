using Dapper;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;
using System.Data.SqlClient;
using System.Text.Json;

namespace SimpleProject.Infrastructure.Persistence.MsSqlServer
{
    public class MsSqlServerDynamicRouteRepository : IDynamicRouteRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerDynamicRouteRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest, DynamicRouteResponse dynamicRouteResponse)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertDynamicRoute]", new
                {
                    method = dynamicRouteRequest.Method,
                    payload = JsonSerializer.Serialize(dynamicRouteResponse.Payload),
                    reference = reference,
                    success = dynamicRouteResponse.Success,
                    type = "DynamicRouteResponse",
                    url = dynamicRouteRequest.Url,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertDynamicRoute]", new
                {
                    method = dynamicRouteRequest.Method,
                    payload = JsonSerializer.Serialize(dynamicRouteRequest.Payload),
                    reference = reference,
                    success = true,
                    type = "DynamicRouteRequest",
                    url = dynamicRouteRequest.Url,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
