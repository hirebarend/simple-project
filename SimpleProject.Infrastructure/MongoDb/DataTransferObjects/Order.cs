using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SimpleProject.Domain.Order;

namespace SimpleProject.Infrastructure.MongoDb.DataTransferObjects
{
    public class Order
    {
        public DateTimeOffset Created { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public string Reference { get; set; }

        public OrderState State { get; set; }

        public short Version { get; set; }

        public DateTimeOffset Updated { get; set; }

        public static Order FromDomain(Domain.Order.Order order)
        {
            return new Order
            {
                Created = order.Created,
                Id = ObjectId.GenerateNewId(),
                Reference = order.Reference,
                State = order.State,
                Version = order.Version,
                Updated = order.Updated,
            };
        }

        public Domain.Order.Order ToDomain()
        {
            return new Domain.Order.Order
            {
                Created = Created,
                Reference = Reference,
                State = State,
                Version = Version,
                Updated = Updated,
            };
        }
    }
}
