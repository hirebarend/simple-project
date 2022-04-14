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
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertDynamicRouteResponse]", new
                {
                    data = string.Empty,
                    reference = reference,
                    message = string.Empty,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task Insert(string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertDynamicRouteRequest]", new
                {
                    reference = reference,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
