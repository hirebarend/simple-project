using SimpleProject.Domain.Transaction;

namespace SimpleProject.Infrastructure.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Insert(Transaction transaction);

        Task<Transaction> Update(Transaction transaction);
    }
}
