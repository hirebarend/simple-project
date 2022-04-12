namespace SimpleProject.Domain.Order
{
    public class Order
    {
        public DateTimeOffset Created { get; set; }

        public string Reference { get; set; }

        public OrderState State { get; set; }

        public short Version { get; set; }

        public DateTimeOffset Updated { get; set; }

        public static Order Create(string reference)
        {
            return new Order
            {
                Created = DateTimeOffset.UtcNow,
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

            return true;
        }

        public bool Complete()
        {
            if (!CanComplete())
            {
                return false;
            }

            State = OrderState.Completed;

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

            return true;
        }

        public bool Processed()
        {
            if (!CanProcessed())
            {
                return false;
            }

            State = OrderState.Processed;

            return true;
        }
    }
}
