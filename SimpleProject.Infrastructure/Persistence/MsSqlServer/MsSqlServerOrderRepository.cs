using Dapper;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Exceptions;
using System.Data.SqlClient;
using System.Text.Json;

namespace SimpleProject.Infrastructure.Persistence.MsSqlServer
{
    public class MsSqlServerOrderRepository : IOrderRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerOrderRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<Order?> Find(Account account, string reference)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Order>("[dbo].[FindOrder]", new
                {
                    reference = reference,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Order> Insert(Account account, Order order)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                return await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertOrder]", new
                {
                    accountReference = account.Reference,
                    metadata = JsonSerializer.Serialize(order.Metadata),
                    productId = order.ProductId,
                    reference = order.Reference,
                    state = order.State,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Order> Update(Account account, Order order)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                var result = await sqlConnection.QueryFirstOrDefaultAsync<Order?>("[dbo].[UpdateOrder]", new
                {
                    reference = order.Reference,
                    state = order.State,
                    version = order.Version,
                }, commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    throw new BusinessException($"unable to find order with reference, '{order.Reference}'");
                }

                return result;
            }
        }
    }
}
