using MongoDB.Driver;
using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;

namespace SimpleProject.Infrastructure.MongoDb
{
    public class MongoDbTransactionRepository : ITransactionRepository
    {
        protected readonly IMongoCollection<DataTransferObjects.Transaction> _mongoCollection;

        public MongoDbTransactionRepository(IMongoCollection<DataTransferObjects.Transaction> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task<Transaction> Insert(Transaction transaction)
        {
            try
            {
                await _mongoCollection.InsertOneAsync(DataTransferObjects.Transaction.FromDomain(transaction));

                return transaction;
            }
            catch
            {
                var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == transaction.Reference);

                var result = await asyncCursor.FirstAsync();

                return result.ToDomain();
            }
        }

        public async Task<Transaction> Update(Transaction transaction)
        {
            var updateResult = await _mongoCollection.UpdateOneAsync(x => x.Reference == transaction.Reference && x.Version == transaction.Version, Builders<DataTransferObjects.Transaction>.Update.Set(x => x.State, transaction.State.ToString()).Inc(x => x.Version, 1));

            if (updateResult.MatchedCount == 0)
            {
                throw new BusinessException($"unable to find order with reference, '{transaction.Reference}'");
            }

            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == transaction.Reference);

            var result = await asyncCursor.FirstAsync();

            return result.ToDomain();
        }
    }
}
