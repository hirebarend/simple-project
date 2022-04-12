using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Interfaces;

namespace SimpleProject.Application.Services
{
    public class TransactionService
    {
        protected readonly IServiceBus _serviceBus;

        protected readonly ITransactionRepository _transactionRepository;

        public TransactionService(IServiceBus serviceBus, ITransactionRepository transactionRepository)
        {
            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));

            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        public async Task Authorize(Order order, Transaction transaction)
        {
            if (transaction.State != TransactionState.Pending)
            {
                // TODO

                return;
            }

            transaction.State = TransactionState.Authorized;

            transaction = await _transactionRepository.Update(transaction);

            if (transaction.State != TransactionState.Authorized)
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Authorized,
            });
        }

        public async Task Create(Order order, Transaction transaction)
        {
            transaction = await _transactionRepository.Insert(transaction);

            if (transaction.State != TransactionState.Pending)
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Created,
            });
        }

        public async Task Settle(Order order, Transaction transaction)
        {
            if (transaction.State != TransactionState.Authorized)
            {
                // TODO

                return;
            }

            transaction.State = TransactionState.Settled;

            transaction = await _transactionRepository.Update(transaction);

            if (transaction.State != TransactionState.Settled)
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Settled,
            });
        }

        public async Task Void(Order order, Transaction transaction)
        {
            if (transaction.State != TransactionState.Authorized)
            {
                // TODO

                return;
            }

            transaction.State = TransactionState.Voided;

            transaction = await _transactionRepository.Update(transaction);

            if (transaction.State != TransactionState.Voided)
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Voided,
            });
        }
    }
}
