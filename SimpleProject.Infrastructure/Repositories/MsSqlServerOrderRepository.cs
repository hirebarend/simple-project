using Dapper;
using SimpleProject.Domain.Order;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;
using SimpleProject.Shared.Misc;
using System.Data.SqlClient;

namespace SimpleProject.Infrastructure.Repositories
{
    public class MsSqlServerOrderRepository : IOrderRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerOrderRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task DeleteAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [dbo].[Orders];");
            }
        }

        public Task<Order?> Find(string reference)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> Insert(Order order)
        {
            ChaosMonkey.Do();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                return await sqlConnection.QueryFirstAsync<Order>("[dbo].[InsertOrder]", new
                {
                    reference = order.Reference,
                    state = order.State,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Order> Update(Order order)
        {
            ChaosMonkey.Do();

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
