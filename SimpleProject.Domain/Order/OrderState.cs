namespace SimpleProject.Domain.Order
{
    public enum OrderState
    {
        Pending,
        Processing,
        Completed,
        Cancelled,
        Refunded,
        Failed,
    }
}
