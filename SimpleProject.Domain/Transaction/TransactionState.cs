namespace SimpleProject.Domain.Transaction
{
    public enum TransactionState
    {
        Pending,
        Authorized,
        Settled,
        Voided,
        Cancelled,
    }
}
