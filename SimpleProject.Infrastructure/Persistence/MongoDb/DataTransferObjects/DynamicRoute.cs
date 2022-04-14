using MongoDB.Bson;

namespace SimpleProject.Infrastructure.Persistence.MongoDb.DataTransferObjects
{
    public class DynamicRoute
    {
        public BsonDocument? Payload { get; set; }

        public string Reference { get; set; }
    }
}
