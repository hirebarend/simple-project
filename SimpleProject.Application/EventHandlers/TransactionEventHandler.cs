
using SimpleProject.Application.Interfaces;
using SimpleProject.Application.Services;
using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;

namespace SimpleProject.Application.EventHandlers
{
    public class TransactionEventHandler
    {
        protected readonly OrderService _orderService;

        protected readonly IServiceBus _serviceBus;

        protected readonly TransactionService _transactionService;

        public TransactionEventHandler(OrderService orderService, IServiceBus serviceBus, TransactionService transactionService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));

            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        public async Task Handle(TransactionEvent transactionEvent)
        {
            Console.WriteLine($"[TransactionEvent] {transactionEvent.Type} {transactionEvent.Transaction.Reference} {transactionEvent.Transaction.State}");

            // Step 3
            if (transactionEvent.Type == TransactionEventType.Create)
            {
                await _transactionService.Create(transactionEvent.Order, transactionEvent.Transaction);

                return;
            }

            // Step 4
            if (transactionEvent.Type == TransactionEventType.Created)
            {
                await _serviceBus.Publish(new TransactionEvent
                {
                    Order = transactionEvent.Order,
                    Transaction = transactionEvent.Transaction,
                    Type = TransactionEventType.Authorize,
                });

                return;
            }

            // Step 5
            if (transactionEvent.Type == TransactionEventType.Authorize)
            {
                await _transactionService.Authorize(transactionEvent.Order, transactionEvent.Transaction);

                return;
            }

            // Step 6
            if (transactionEvent.Type == TransactionEventType.Authorized)
            {
                await _serviceBus.Publish(new OrderEvent
                {
                    Order = transactionEvent.Order,
                    Transaction = transactionEvent.Transaction,
                    Type = OrderEventType.Process,
                });

                return;
            }

            // Step 9
            if (transactionEvent.Type == TransactionEventType.Settle)
            {
                await _transactionService.Settle(transactionEvent.Order, transactionEvent.Transaction);

                return;
            }

            // Step 10
            if (transactionEvent.Type == TransactionEventType.Settled)
            {
                await _serviceBus.Publish(new OrderEvent
                {
                    Order = transactionEvent.Order,
                    Transaction = transactionEvent.Transaction,
                    Type = OrderEventType.Complete,
                });

                return;
            }
        }
    }
}
