using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Enums;
using SimpleProject.Domain.Events;
using SimpleProject.Domain.ValueObjects;

namespace SimpleProject.Infrastructure.Services
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

        public async Task Authorize(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
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
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Authorized,
            });
        }

        public async Task Cancel(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
        {
            if (transaction.State != TransactionState.Pending)
            {
                // TODO

                return;
            }

            transaction.State = TransactionState.Cancelled;

            transaction = await _transactionRepository.Update(transaction);

            if (transaction.State != TransactionState.Cancelled)
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Cancelled,
            });
        }

        public async Task Create(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
        {
            if (transaction.State != TransactionState.Initial)
            {
                // TODO

                return;
            }

            transaction.State = TransactionState.Pending;

            transaction = await _transactionRepository.Insert(transaction);

            if (transaction.State != TransactionState.Pending)
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Created,
            });
        }

        public async Task Settle(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
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
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Settled,
            });
        }

        public async Task Void(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
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
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = TransactionEventType.Voided,
            });
        }
    }
}
