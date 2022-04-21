using MongoDB.Bson;
using MongoDB.Driver;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;
using SimpleProject.Infrastructure.Persistence.MongoDb.DataTransferObjects;
using System.Text.Json;

namespace SimpleProject.Infrastructure.Persistence.MongoDb
{
    public class MongoDbDynamicRouteRepository : IDynamicRouteRepository
    {
        protected readonly IMongoCollection<DynamicRoute> _mongoCollection;

        public MongoDbDynamicRouteRepository(IMongoCollection<DynamicRoute> mongoCollection)
        {
            _mongoCollection = mongoCollection ?? throw new ArgumentNullException(nameof(mongoCollection));
        }

        public async Task<DynamicRouteResponse?> FindResponse(Account account, string reference)
        {
            var asyncCursor = await _mongoCollection.FindAsync(x => x.Reference == reference);

            var result = await asyncCursor.FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            return new DynamicRouteResponse
            {
                Payload = result.Payload,
                Success = result.Success,
            };
        }

        public async Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest, DynamicRouteResponse dynamicRouteResponse)
        {
            var json = dynamicRouteResponse.Payload == null ? null : JsonSerializer.Serialize(dynamicRouteResponse.Payload);

            await _mongoCollection.InsertOneAsync(new DynamicRoute
            {
                Method = dynamicRouteRequest.Method,
                Payload = string.IsNullOrWhiteSpace(json) ? null : BsonDocument.Parse(json),
                Reference = reference,
                Success = dynamicRouteResponse.Success,
                Type = "DynamicRouteResponse",
                Url = dynamicRouteRequest.Url,
            });
        }

        public async Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            var json = dynamicRouteRequest.Payload == null ? null : JsonSerializer.Serialize(dynamicRouteRequest.Payload);

            await _mongoCollection.InsertOneAsync(new DynamicRoute
            {
                Method = dynamicRouteRequest.Method,
                Payload = string.IsNullOrWhiteSpace(json) ? null : BsonDocument.Parse(json),
                Reference = reference,
                Success = true,
                Type = "DynamicRouteRequest",
                Url = dynamicRouteRequest.Url,
            });
        }
    }
}
