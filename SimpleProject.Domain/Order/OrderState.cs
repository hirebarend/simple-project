namespace SimpleProject.Domain.Order
{
    public enum OrderState
    {
        Pending,
        Processing,
        Processed,
        Completed,
        Cancelled,
        Refunded,
        Failed,
    }
}
