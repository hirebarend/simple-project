using MongoDB.Driver;
using SimpleProject.Domain.Order;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;

namespace SimpleProject.Infrastructure.MongoDb
{
    public class MongoDbOrderRepository : IOrderRepository
    {
        protected readonly IMongoCollection<DataTransferObjects.Order> _mongoCollection;

        public MongoDbOrderRepository(IMongoCollection<DataTransferObjects.Order> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task<Order> Insert(Order order)
        {
            try
            {
                await _mongoCollection.InsertOneAsync(DataTransferObjects.Order.FromDomain(order));

                return order;
            }
            catch
            {
                var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == order.Reference);

                var result = await asyncCursor.FirstAsync();

                return result.ToDomain();
            }
        }

        public async Task<Order> Update(Order order)
        {
            var updateResult = await _mongoCollection.UpdateOneAsync(x => x.Reference == order.Reference && x.Version == order.Version, Builders<DataTransferObjects.Order>.Update.Set(x => x.State, order.State).Inc(x => x.Version, 1));

            if (updateResult.MatchedCount == 0)
            {
                throw new BusinessException($"unable to find order with reference, '{order.Reference}'");
            }

            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == order.Reference);

            var result = await asyncCursor.FirstAsync();

            return result.ToDomain();
        }
    }
}
