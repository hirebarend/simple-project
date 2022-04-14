using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SimpleProject.Domain.Enums;

namespace SimpleProject.Infrastructure.Persistence.MongoDb.DataTransferObjects
{
    public class Transaction
    {
        public int Amount { get; set; }

        public string Created { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public string Reference { get; set; }

        public string State { get; set; }

        public string Updated { get; set; }

        public short Version { get; set; }

        public static Transaction FromDomain(Domain.Entities.Transaction transaction)
        {
            return new Transaction
            {
                Amount = transaction.Amount,
                Created = transaction.Created.ToString("o"),
                Id = ObjectId.GenerateNewId(),
                Reference = transaction.Reference,
                State = transaction.State.ToString(),
                Version = transaction.Version,
                Updated = transaction.Updated.ToString("o"),
            };
        }

        public Domain.Entities.Transaction ToDomain()
        {
            return new Domain.Entities.Transaction
            {
                Amount = Amount,
                Created = DateTimeOffset.Parse(Created),
                Reference = Reference,
                State = Enum.Parse<TransactionState>(State),
                Version = Version,
                Updated = DateTimeOffset.Parse(Updated),
            };
        }
    }
}
