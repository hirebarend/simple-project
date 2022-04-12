namespace SimpleProject.Domain.Transaction
{
    public enum TransactionEventType
    {
        Create,
        Created,
        Authorize,
        Authorized,
        Settle,
        Settled,
        Void,
        Voided,
        Cancel,
        Cancelled
    }
}
