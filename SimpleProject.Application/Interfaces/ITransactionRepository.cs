using SimpleProject.Domain.Entities;

namespace SimpleProject.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Authorize(Account account, Transaction transaction);

        Task<Transaction> Insert(Account account, Transaction transaction);

        Task<Transaction> Update(Account account, Transaction transaction);

        Task<Transaction> Void(Account account, Transaction transaction);
    }
}
