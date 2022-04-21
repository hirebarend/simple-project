using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SimpleProject.Domain.Enums;

namespace SimpleProject.Infrastructure.Persistence.MongoDb.DataTransferObjects
{
    public class Order
    {
        public string AccountReference { get; set; }

        public string Created { get; set; }

        public double Duration { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        public string ProductId { get; set; }

        public string Reference { get; set; }

        public string State { get; set; }

        public short Version { get; set; }

        public string Updated { get; set; }

        public static Order FromDomain(Domain.Entities.Account account, Domain.Entities.Order order)
        {
            return new Order
            {
                AccountReference = account.Reference,
                Created = order.Created.ToString("o"),
                Duration = order.Updated.Subtract(order.Created).TotalMilliseconds,
                Id = ObjectId.GenerateNewId(),
                Metadata = order.Metadata,
                ProductId = order.ProductId,
                Reference = order.Reference,
                State = order.State.ToString(),
                Version = order.Version,
                Updated = order.Updated.ToString("o"),
            };
        }

        public Domain.Entities.Order ToDomain()
        {
            return new Domain.Entities.Order
            {
                Created = DateTimeOffset.Parse(Created),
                Metadata = Metadata,
                ProductId = ProductId,
                Reference = Reference,
                State = Enum.Parse<OrderState>(State),
                Version = Version,
                Updated = DateTimeOffset.Parse(Updated),
            };
        }
    }
}
