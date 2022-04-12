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

            switch (transactionEvent.Type)
            {
                case TransactionEventType.Authorize:
                    await HandleAuthorize(transactionEvent);

                    break;
                case TransactionEventType.Authorized:
                    await HandleAuthorized(transactionEvent);

                    break;
                case TransactionEventType.Cancel:
                    break;
                case TransactionEventType.Cancelled:
                    break;
                case TransactionEventType.Create:
                    await HandleCreate(transactionEvent);

                    break;
                case TransactionEventType.Created:
                    await HandleCreated(transactionEvent);

                    break;
                case TransactionEventType.Settle:
                    await HandleSettle(transactionEvent);

                    break;
                case TransactionEventType.Settled:
                    break;
                case TransactionEventType.Void:
                    await HandleVoid(transactionEvent);

                    break;
                case TransactionEventType.Voided:
                    break;
            }
        }

        public async Task HandleAuthorize(TransactionEvent transactionEvent)
        {
            await _transactionService.Authorize(transactionEvent.Order, transactionEvent.Transaction);
        }

        public async Task HandleAuthorized(TransactionEvent transactionEvent)
        {
            await _serviceBus.Publish(new OrderEvent
            {
                Order = transactionEvent.Order,
                Transaction = transactionEvent.Transaction,
                Type = OrderEventType.Process,
            });
        }

        public async Task HandleCreate(TransactionEvent transactionEvent)
        {
            await _transactionService.Create(transactionEvent.Order, transactionEvent.Transaction);
        }

        public async Task HandleCreated(TransactionEvent transactionEvent)
        {
            await _serviceBus.Publish(new TransactionEvent
            {
                Order = transactionEvent.Order,
                Transaction = transactionEvent.Transaction,
                Type = TransactionEventType.Authorize,
            });
        }

        public async Task HandleSettle(TransactionEvent transactionEvent)
        {
            await _transactionService.Settle(transactionEvent.Order, transactionEvent.Transaction);
        }

        public async Task HandleVoid(TransactionEvent transactionEvent)
        {
            await _transactionService.Void(transactionEvent.Order, transactionEvent.Transaction);
        }
    }
}
