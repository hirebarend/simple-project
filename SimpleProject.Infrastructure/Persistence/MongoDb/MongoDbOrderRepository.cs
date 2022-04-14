using MongoDB.Driver;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Exceptions;

namespace SimpleProject.Infrastructure.Persistence.MongoDb
{
    public class MongoDbOrderRepository : IOrderRepository
    {
        protected readonly IMongoCollection<DataTransferObjects.Order> _mongoCollection;

        public MongoDbOrderRepository(IMongoCollection<DataTransferObjects.Order> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task<Order?> Find(string reference)
        {
            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == reference);

            var result = await asyncCursor.FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            return result.ToDomain();
        }

        public async Task<Order> Insert(Order order)
        {
            var orderDataTransferObject = DataTransferObjects.Order.FromDomain(order);

            try
            {
                await _mongoCollection.InsertOneAsync(orderDataTransferObject);

                return order;
            }
            catch
            {
                var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == orderDataTransferObject.Reference);

                var result = await asyncCursor.FirstAsync();

                return result.ToDomain();
            }
        }

        public async Task<Order> Update(Order order)
        {
            var orderDataTransferObject = DataTransferObjects.Order.FromDomain(order);

            var updateResult = await _mongoCollection.UpdateOneAsync(x => x.Reference == orderDataTransferObject.Reference && x.Version == orderDataTransferObject.Version, Builders<DataTransferObjects.Order>.Update.Set(x => x.State, orderDataTransferObject.State).Inc(x => x.Version, 1).Set(x => x.Updated, orderDataTransferObject.Updated));

            if (updateResult.MatchedCount == 0)
            {
                throw new BusinessException($"unable to find order with reference, '{orderDataTransferObject.Reference}'");
            }

            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == orderDataTransferObject.Reference);

            var result = await asyncCursor.FirstAsync();

            return result.ToDomain();
        }
    }
}
