using Dapper;
using SimpleProject.Domain.Order;
using SimpleProject.Infrastructure.Interfaces;
using System.Data.SqlClient;

namespace SimpleProject.Infrastructure.Repositories
{
    public class MsSqlServerProductGatewayLogRepository : IProductGatewayLogRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerProductGatewayLogRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task DeleteAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [dbo].[Logs];");
            }
        }

        public async Task Insert(string reference)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertLog]", new
                {
                    reference = reference,
                    message = "ProductGateway.Purchase",
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
