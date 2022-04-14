using MongoDB.Bson;
using MongoDB.Driver;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.ValueObjects;
using SimpleProject.Infrastructure.Persistence.MongoDb.DataTransferObjects;

namespace SimpleProject.Infrastructure.Persistence.MongoDb
{
    public class MongoDbDynamicRouteRepository : IDynamicRouteRepository
    {
        protected readonly IMongoCollection<DynamicRoute> _mongoCollection;

        public MongoDbDynamicRouteRepository(IMongoCollection<DynamicRoute> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task Insert(string reference, DynamicRouteResponse dynamicRouteResponse)
        {
            var json = dynamicRouteResponse.Payload == null ? null : System.Text.Json.JsonSerializer.Serialize(dynamicRouteResponse.Payload);

            await _mongoCollection.InsertOneAsync(new DynamicRoute
            {
                Payload = string.IsNullOrWhiteSpace(json) ? null : BsonDocument.Parse(json),
                Reference = reference,
            });
        }

        public async Task Insert(string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            var json = dynamicRouteRequest.Payload == null ? null : System.Text.Json.JsonSerializer.Serialize(dynamicRouteRequest.Payload);

            await _mongoCollection.InsertOneAsync(new DynamicRoute
            {
                Payload = string.IsNullOrWhiteSpace(json) ? null : BsonDocument.Parse(json),
                Reference = reference,
            });
        }
    }
}
