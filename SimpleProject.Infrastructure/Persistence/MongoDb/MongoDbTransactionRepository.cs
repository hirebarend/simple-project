using MongoDB.Driver;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Exceptions;

namespace SimpleProject.Infrastructure.Persistence.MongoDb
{
    public class MongoDbTransactionRepository : ITransactionRepository
    {
        protected readonly IMongoCollection<DataTransferObjects.Transaction> _mongoCollection;

        public MongoDbTransactionRepository(IMongoCollection<DataTransferObjects.Transaction> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task<Transaction> Authorize(Account account, Transaction transaction)
        {
            var transactionDataTransferObject = DataTransferObjects.Transaction.FromDomain(account, transaction);

            var updateResult = await _mongoCollection.UpdateOneAsync(x => x.Reference == transactionDataTransferObject.Reference && x.Version == transactionDataTransferObject.Version, Builders<DataTransferObjects.Transaction>.Update.Set(x => x.State, transactionDataTransferObject.State).Inc(x => x.Version, 1).Set(x => x.Updated, transactionDataTransferObject.Updated));

            if (updateResult.MatchedCount == 0)
            {
                throw new BusinessException($"unable to find order with reference, '{transactionDataTransferObject.Reference}'");
            }

            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == transactionDataTransferObject.Reference);

            var result = await asyncCursor.FirstAsync();

            return result.ToDomain();
        }

        public async Task<Transaction> Insert(Account account, Transaction transaction)
        {
            var transactionDataTransferObject = DataTransferObjects.Transaction.FromDomain(account, transaction);

            try
            {
                await _mongoCollection.InsertOneAsync(transactionDataTransferObject);

                return transaction;
            }
            catch
            {
                var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == transactionDataTransferObject.Reference);

                var result = await asyncCursor.FirstAsync();

                return result.ToDomain();
            }
        }

        public async Task<Transaction> Update(Account account, Transaction transaction)
        {
            var transactionDataTransferObject = DataTransferObjects.Transaction.FromDomain(account, transaction);

            var updateResult = await _mongoCollection.UpdateOneAsync(x => x.Reference == transactionDataTransferObject.Reference && x.Version == transactionDataTransferObject.Version, Builders<DataTransferObjects.Transaction>.Update.Set(x => x.State, transactionDataTransferObject.State).Inc(x => x.Version, 1).Set(x => x.Updated, transactionDataTransferObject.Updated));

            if (updateResult.MatchedCount == 0)
            {
                throw new BusinessException($"unable to find order with reference, '{transactionDataTransferObject.Reference}'");
            }

            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == transactionDataTransferObject.Reference);

            var result = await asyncCursor.FirstAsync();

            return result.ToDomain();
        }

        public async Task<Transaction> Void(Account account, Transaction transaction)
        {
            var transactionDataTransferObject = DataTransferObjects.Transaction.FromDomain(account, transaction);

            var updateResult = await _mongoCollection.UpdateOneAsync(x => x.Reference == transactionDataTransferObject.Reference && x.Version == transactionDataTransferObject.Version, Builders<DataTransferObjects.Transaction>.Update.Set(x => x.State, transactionDataTransferObject.State).Inc(x => x.Version, 1).Set(x => x.Updated, transactionDataTransferObject.Updated));

            if (updateResult.MatchedCount == 0)
            {
                throw new BusinessException($"unable to find order with reference, '{transactionDataTransferObject.Reference}'");
            }

            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == transactionDataTransferObject.Reference);

            var result = await asyncCursor.FirstAsync();

            return result.ToDomain();
        }
    }
}
