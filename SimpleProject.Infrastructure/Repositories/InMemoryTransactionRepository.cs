using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;
using SimpleProject.Shared.Misc;

namespace SimpleProject.Infrastructure.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        protected object _lock = new object();

        public readonly IList<Transaction> _transactions = new List<Transaction>();

        public Task<Transaction> Insert(Transaction transaction)
        {
            ChaosMonkey.Do();

            lock (_lock)
            {
                var transactionExisting = _transactions.FirstOrDefault(x => x.Reference == transaction.Reference);

                if (transactionExisting != null)
                {
                    return Task.FromResult(transactionExisting);
                }

                _transactions.Add(transaction);

                ChaosMonkey.Do();

                return Task.FromResult(transaction);
            }
        }

        public Task<Transaction> Update(Transaction transaction)
        {
            ChaosMonkey.Do();

            lock (_lock)
            {
                var transactionExisting = _transactions.FirstOrDefault(x => x.Reference == transaction.Reference);

                if (transactionExisting == null)
                {
                    throw new BusinessException($"unable to find transaction with reference, '{transaction.Reference}'");
                }

                if (transactionExisting.Version != transaction.Version)
                {
                    return Task.FromResult(transactionExisting);
                }

                transactionExisting.Reference = transaction.Reference;
                transactionExisting.State = transaction.State;
                transactionExisting.Version = transaction.Version + 1;
                transactionExisting.Updated = DateTimeOffset.UtcNow;

                ChaosMonkey.Do();

                return Task.FromResult(transactionExisting);
            }
        }
    }
}
