namespace SimpleProject.Domain.Order
{
    public class OrderEvent
    {
        public Order Order { get; set; }

        public Transaction.Transaction Transaction { get; set; }

        public OrderEventType Type { get; set; }
    }
}
