using Dapper;
using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;
using SimpleProject.Shared.Misc;
using System.Data.SqlClient;

namespace SimpleProject.Infrastructure.Repositories
{
    public class MsSqlServerTransactionRepository : ITransactionRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerTransactionRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task DeleteAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [dbo].[Transactions];");
            }
        }

        public async Task<Transaction> Insert(Transaction transaction)
        {
            ChaosMonkey.Do();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                return await sqlConnection.QueryFirstAsync<Transaction>("[dbo].[InsertTransaction]", new
                {
                    amount = transaction.Amount,
                    reference = transaction.Reference,
                    state = transaction.State,
                }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Transaction> Update(Transaction transaction)
        {
            ChaosMonkey.Do();

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                var result = await sqlConnection.QueryFirstOrDefaultAsync<Transaction?>("[dbo].[UpdateTransaction]", new
                {
                    reference = transaction.Reference,
                    state = transaction.State,
                    version = transaction.Version,
                }, commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    throw new BusinessException($"unable to find order with reference, '{transaction.Reference}'");
                }

                return result;
            }
        }
    }
}
