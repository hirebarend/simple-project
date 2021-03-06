using SimpleProject.Domain.Enums;

namespace SimpleProject.Domain.Entities
{
    public class Order
    {
        public DateTimeOffset Created { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        public string ProductId { get; set; }

        public string Reference { get; set; }

        public OrderState State { get; set; }

        public short Version { get; set; }

        public DateTimeOffset Updated { get; set; }

        public static Order Create(string reference)
        {
            return new Order
            {
                Created = DateTimeOffset.UtcNow,
                Metadata = new Dictionary<string, string>
                {
                    { "reference", reference},
                },
                ProductId = "6317",
                Reference = reference,
                State = OrderState.Pending,
                Version = 0,
                Updated = DateTimeOffset.UtcNow,
            };
        }

        public bool CanCancel()
        {
            return true;
        }

        public bool CanComplete()
        {
            return IsProcessed();
        }

        public bool CanProcess()
        {
            return IsPending();
        }

        public bool CanProcessed()
        {
            return IsProcessing();
        }

        public bool Cancel()
        {
            if (!CanCancel())
            {
                return false;
            }

            State = OrderState.Cancelled;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }

        public bool Complete()
        {
            if (!CanComplete())
            {
                return false;
            }

            State = OrderState.Completed;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }

        public bool IsCancelled()
        {
            return State == OrderState.Cancelled;
        }

        public bool IsCompleted()
        {
            return State == OrderState.Completed;
        }

        public bool IsPending()
        {
            return State == OrderState.Pending;
        }

        public bool IsProcessed()
        {
            return State == OrderState.Processed;
        }

        public bool IsProcessing()
        {
            return State == OrderState.Processing;
        }

        public bool Process()
        {
            if (!CanProcess())
            {
                return false;
            }

            State = OrderState.Processing;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }

        public bool Processed()
        {
            if (!CanProcessed())
            {
                return false;
            }

            State = OrderState.Processed;

            Updated = DateTimeOffset.UtcNow;

            return true;
        }
    }
}
