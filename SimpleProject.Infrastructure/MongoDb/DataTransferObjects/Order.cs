using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SimpleProject.Domain.Order;

namespace SimpleProject.Infrastructure.MongoDb.DataTransferObjects
{
    public class Order
    {
        public string Created { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public string Reference { get; set; }

        public string State { get; set; }

        public short Version { get; set; }

        public string Updated { get; set; }

        public static Order FromDomain(Domain.Order.Order order)
        {
            return new Order
            {
                Created = order.Created.ToString("o"),
                Id = ObjectId.GenerateNewId(),
                Reference = order.Reference,
                State = order.State.ToString(),
                Version = order.Version,
                Updated = order.Updated.ToString("o"),
            };
        }

        public Domain.Order.Order ToDomain()
        {
            return new Domain.Order.Order
            {
                Created = DateTimeOffset.Parse(Created),
                Reference = Reference,
                State = Enum.Parse<OrderState>(State),
                Version = Version,
                Updated = DateTimeOffset.Parse(Updated),
            };
        }
    }
}
