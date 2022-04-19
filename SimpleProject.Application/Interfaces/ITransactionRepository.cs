using SimpleProject.Domain.Entities;

namespace SimpleProject.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Authorize(Transaction transaction);

        Task<Transaction> Insert(Transaction transaction);

        Task<Transaction> Update(Transaction transaction);

        Task<Transaction> Void(Transaction transaction);
    }
}
