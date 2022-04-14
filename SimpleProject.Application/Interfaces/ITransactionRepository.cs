using SimpleProject.Domain.Entities;

namespace SimpleProject.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> Insert(Transaction transaction);

        Task<Transaction> Update(Transaction transaction);
    }
}
