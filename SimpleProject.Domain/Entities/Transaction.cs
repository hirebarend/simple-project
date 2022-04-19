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

        public bool Authorize()
        {
            if (!CanAuthorize())
            {
                return false;
            }

            State = TransactionState.Authorized;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }

        public bool CanAuthorize()
        {
            return IsPending();
        }

        public bool CanCancel()
        {
            return IsPending();
        }

        public bool Cancel()
        {
            if (!CanCancel())
            {
                return false;
            }

            State = TransactionState.Cancelled;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }

        public bool CanSettle()
        {
            return IsAuthorized();
        }

        public bool CanVoid()
        {
            return IsAuthorized();
        }

        public bool IsAuthorized()
        {
            return State == TransactionState.Authorized;
        }

        public bool IsCancelled()
        {
            return State == TransactionState.Cancelled;
        }

        public bool IsInitial()
        {
            return State == TransactionState.Initial;
        }

        public bool IsPending()
        {
            return State == TransactionState.Pending;
        }

        public bool IsSettled()
        {
            return State == TransactionState.Settled;
        }

        public bool IsVoided()
        {
            return State == TransactionState.Voided;
        }

        public bool Settle()
        {
            if (!CanSettle())
            {
                return false;
            }

            State = TransactionState.Settled;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }

        public bool Void()
        {
            if (!CanVoid())
            {
                return false;
            }

            State = TransactionState.Voided;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }
    }
}
