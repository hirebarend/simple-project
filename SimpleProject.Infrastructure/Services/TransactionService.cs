using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Enums;
using SimpleProject.Domain.Events;
using SimpleProject.Domain.Exceptions;
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
            if (!transaction.Authorize())
            {
                // TODO

                return;
            }

            try
            {
                transaction = await _transactionRepository.Authorize(account, transaction);
            }catch (InsufficientBalanceException insufficientBalanceException)
            {
                transaction.State = TransactionState.Pending;

                await _serviceBus.Publish(new OrderEvent
                {
                    Account = account,
                    DynamicRouteRequest = dynamicRouteRequest,
                    Order = order,
                    Transaction = transaction,
                    Type = OrderEventType.Cancel,
                });

                return;
            }

            if (!transaction.IsAuthorized())
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
            if (!transaction.Cancel())
            {
                // TODO

                return;
            }

            transaction = await _transactionRepository.Update(account, transaction);

            if (!transaction.IsCancelled())
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
            if (!transaction.IsInitial())
            {
                // TODO

                return;
            }

            transaction.State = TransactionState.Pending;

            transaction = await _transactionRepository.Insert(account, transaction);

            if (!transaction.IsPending())
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
            if (!transaction.Settle())
            {
                // TODO

                return;
            }

            transaction = await _transactionRepository.Update(account, transaction);

            if (!transaction.IsSettled())
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
            if (!transaction.Void())
            {
                // TODO

                return;
            }

            transaction = await _transactionRepository.Void(account, transaction);

            if (!transaction.IsVoided())
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
