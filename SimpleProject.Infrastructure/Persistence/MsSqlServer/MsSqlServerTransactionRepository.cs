using Dapper;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Exceptions;
using System.Data.SqlClient;

namespace SimpleProject.Infrastructure.Persistence.MsSqlServer
{
    public class MsSqlServerTransactionRepository : ITransactionRepository
    {
        protected readonly string _connectionString;

        public MsSqlServerTransactionRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<Transaction> Authorize(Transaction transaction)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    var result = await sqlConnection.QueryFirstOrDefaultAsync<Transaction?>("[dbo].[AuthorizeTransaction]", new
                    {
                        amount = transaction.Amount,
                        reference = transaction.Reference,
                        version = transaction.Version,
                    }, commandType: System.Data.CommandType.StoredProcedure);

                    if (result == null)
                    {
                        throw new BusinessException($"unable to find transaction with reference, '{transaction.Reference}'");
                    }

                    return result;
                }
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Message.Equals("insufficient_balance"))
                {
                    throw new InsufficientBalanceException();
                }

                throw;
            }
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
                    throw new BusinessException($"unable to find transaction with reference, '{transaction.Reference}'");
                }

                return result;
            }
        }

        public async Task<Transaction> Void(Transaction transaction)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                var result = await sqlConnection.QueryFirstOrDefaultAsync<Transaction?>("[dbo].[VoidTransaction]", new
                {
                    amount = transaction.Amount,
                    reference = transaction.Reference,
                    version = transaction.Version,
                }, commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    throw new BusinessException($"unable to find transaction with reference, '{transaction.Reference}'");
                }

                return result;
            }
        }
    }
}
