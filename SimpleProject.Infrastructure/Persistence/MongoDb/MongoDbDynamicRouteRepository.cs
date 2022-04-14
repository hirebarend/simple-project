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
            await _mongoCollection.InsertOneAsync(new DynamicRoute
            {
                Payload = dynamicRouteResponse.Payload,
                //Reference = reference,
                //Success = dynamicRouteResponse.Success,
            });
        }

        public async Task Insert(string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            await _mongoCollection.InsertOneAsync(new DynamicRoute
            {
                Payload = dynamicRouteRequest.Payload,
                //Reference = reference,
            });
        }
    }
}
