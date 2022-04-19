using Dapper;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;
using System.Data.SqlClient;

namespace SimpleProject.Infrastructure.Persistence.MsSqlServer
{
    public class MsSqlServerDynamicRouteRepository : IDynamicRouteRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerDynamicRouteRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task Insert(string reference, DynamicRouteResponse dynamicRouteResponse)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertDynamicRoute]", new
                {
                    data = System.Text.Json.JsonSerializer.Serialize(dynamicRouteResponse.Payload),
                    reference = reference,
                    message = "DynamicRouteResponse",
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task Insert(string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertDynamicRoute]", new
                {
                    data = System.Text.Json.JsonSerializer.Serialize(dynamicRouteRequest.Payload),
                    reference = reference,
                    message = "DynamicRouteRequest",
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
