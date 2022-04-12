using SimpleProject.Domain.Order;

namespace SimpleProject.Domain.Transaction
{
    public class TransactionEvent
    {
        public Order.Order Order { get; set; }

        public Transaction Transaction { get; set; }

        public TransactionEventType Type { get; set; }
    }
}
