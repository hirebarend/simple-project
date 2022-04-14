using SimpleProject.Domain.Enums;

namespace SimpleProject.Domain.Entities
{
    public class Transaction
    {
        public int Amount { get; set; }

        public DateTimeOffset Created { get; set; }

        public string Reference { get; set; }

        public TransactionState State { get; set; }

        public DateTimeOffset Updated { get; set; }

        public short Version { get; set; }

        public static Transaction Create(int amount, string reference)
        {
            return new Transaction
            {
                Amount = amount,
                Created = DateTimeOffset.UtcNow,
                Reference = reference,
                State = TransactionState.Initial,
                Updated = DateTimeOffset.UtcNow,
                Version = 0,
            };
        }

        public Transaction Authorize()
        {
            if (State == TransactionState.Pending)
            {
                State = TransactionState.Authorized;
            }

            return this;
        }

        public Transaction Cancel()
        {
            if (State == TransactionState.Pending)
            {
                State = TransactionState.Cancelled;
            }

            return this;
        }

        public Transaction Settle()
        {
            if (State == TransactionState.Authorized)
            {
                State = TransactionState.Settled;
            }

            return this;
        }

        public Transaction Void()
        {
            if (State == TransactionState.Authorized || State == TransactionState.Settled)
            {
                State = TransactionState.Voided;
            }

            return this;
        }
    }
}
