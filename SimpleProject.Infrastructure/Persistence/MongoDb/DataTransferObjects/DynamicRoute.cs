using MongoDB.Bson;

namespace SimpleProject.Infrastructure.Persistence.MongoDb.DataTransferObjects
{
    public class DynamicRoute
    {
        public string Method { get; set; }

        public BsonDocument? Payload { get; set; }

        public string Reference { get; set; }

        public bool Success { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }
    }
}
