using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;
using SimpleProject.Shared.Misc;

namespace SimpleProject.Infrastructure.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        protected long _balance = 0;

        protected object _lock = new object();

        protected readonly IList<Transaction> _transactions = new List<Transaction>();

        public Task<Transaction> Insert(Transaction transaction)
        {
            lock (_lock)
            {
                ChaosMonkey.Do();

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
            lock (_lock)
            {
                ChaosMonkey.Do();

                var transactionExisting = _transactions.FirstOrDefault(x => x.Reference == transaction.Reference);

                if (transactionExisting == null)
                {
                    _transactions.Add(transaction);

                    return Task.FromResult(transaction);

                    // throw new BusinessException($"unable to find transaction with reference, '{transaction.Reference}'");
                }

                if (transactionExisting.Version != transaction.Version)
                {
                    return Task.FromResult(transactionExisting);
                }

                if (transaction.State == TransactionState.Authorized)
                {
                    _balance += transaction.Amount;

                    Console.WriteLine($"_balance: {_balance}");
                } else if (transaction.State == TransactionState.Voided)
                {
                    _balance -= transaction.Amount;

                    Console.WriteLine($"_balance: {_balance}");
                }

                transactionExisting.Reference = transaction.Reference;
                transactionExisting.State = transaction.State;
                transactionExisting.Version = Convert.ToInt16(transaction.Version + 1);
                transactionExisting.Updated = DateTimeOffset.UtcNow;

                ChaosMonkey.Do();

                return Task.FromResult(transactionExisting);
            }
        }
    }
}
