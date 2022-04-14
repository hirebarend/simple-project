using MongoDB.Bson;
using MongoDB.Driver;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.ValueObjects;

namespace SimpleProject.Infrastructure.Persistence.MongoDb
{
    public class MongoDbDynamicRouteRepository : IDynamicRouteRepository
    {
        protected readonly IMongoCollection<object> _mongoCollection;

        public MongoDbDynamicRouteRepository(IMongoCollection<object> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task Insert(string reference, DynamicRouteResponse dynamicRouteResponse)
        {
            await _mongoCollection.InsertOneAsync(new
            {
                Payload = BsonDocument.Create(dynamicRouteResponse.Payload),
            });
        }

        public async Task Insert(string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            await _mongoCollection.InsertOneAsync(new
            {
                Payload = BsonDocument.Create(dynamicRouteRequest.Payload),
            });
        }
    }
}
