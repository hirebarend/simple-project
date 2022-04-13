using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SimpleProject.Domain.Transaction;

namespace SimpleProject.Infrastructure.MongoDb.DataTransferObjects
{
    public class Transaction
    {
        public int Amount { get; set; }

        public DateTimeOffset Created { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public string Reference { get; set; }

        public TransactionState State { get; set; }

        public DateTimeOffset Updated { get; set; }

        public short Version { get; set; }

        public static Transaction FromDomain(Domain.Transaction.Transaction transaction)
        {
            return new Transaction
            {
                Amount = transaction.Amount,
                Created = transaction.Created,
                Id = ObjectId.GenerateNewId(),
                Reference = transaction.Reference,
                State = transaction.State,
                Version = transaction.Version,
                Updated = transaction.Updated,
            };
        }

        public Domain.Transaction.Transaction ToDomain()
        {
            return new Domain.Transaction.Transaction
            {
                Amount = Amount,
                Created = Created,
                Reference = Reference,
                State = State,
                Version = Version,
                Updated = Updated,
            };
        }
    }
}
