using SimpleProject.Application.Interfaces;
using SimpleProject.Application.Services;
using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;

namespace SimpleProject.Application.EventHandlers
{
    public class OrderEventHandler
    {
        protected readonly OrderService _orderService;

        protected readonly IServiceBus _serviceBus;

        protected readonly TransactionService _transactionService;

        public OrderEventHandler(OrderService orderService, IServiceBus serviceBus, TransactionService transactionService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));

            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        }

        public async Task Handle(OrderEvent orderEvent)
        {
            Console.WriteLine($"[OrderEvent] {orderEvent.Type} {orderEvent.Order.Reference} {orderEvent.Order.State}");

            switch (orderEvent.Type)
            {
                case OrderEventType.Cancel:
                    await HandleCancel(orderEvent);

                    break;
                case OrderEventType.Cancelled:
                    await HandleCancelled(orderEvent);

                    break;
                case OrderEventType.Complete:
                    await HandleComplete(orderEvent);

                    break;
                case OrderEventType.Completed:
                    await HandleCompleted(orderEvent);

                    break;
                case OrderEventType.Create:
                    await HandleCreate(orderEvent);

                    break;
                case OrderEventType.Created:
                    await HandleCreated(orderEvent);

                    break;
                case OrderEventType.Process:
                    await HandleProcess(orderEvent);

                    break;
                case OrderEventType.Processed:
                    break;
                case OrderEventType.Processing:
                    break;
            }
        }

        public async Task HandleCancel(OrderEvent orderEvent)
        {
            await _orderService.Cancel(orderEvent.Order, orderEvent.Transaction);
        }

        public async Task HandleCancelled(OrderEvent orderEvent)
        {
            if (orderEvent.Transaction == null)
            {
                return;
            }

            if (orderEvent.Transaction.State == TransactionState.Authorized)
            {
                await _serviceBus.Publish(new TransactionEvent
                {
                    Order = orderEvent.Order,
                    Transaction = orderEvent.Transaction,
                    Type = TransactionEventType.Void,
                });

                return;
            }

            await _serviceBus.Publish(new TransactionEvent
            {
                Order = orderEvent.Order,
                Transaction = orderEvent.Transaction,
                Type = TransactionEventType.Cancel,
            });
        }

        public async Task HandleComplete(OrderEvent orderEvent)
        {
            await _orderService.Complete(orderEvent.Order, orderEvent.Transaction);
        }

        public async Task HandleCompleted(OrderEvent orderEvent)
        {
            await _serviceBus.Publish(new TransactionEvent
            {
                Order = orderEvent.Order,
                Transaction = orderEvent.Transaction,
                Type = TransactionEventType.Settle,
            });
        }

        public async Task HandleCreate(OrderEvent orderEvent)
        {
            await _orderService.Create(orderEvent.Order, orderEvent.Transaction);
        }

        public async Task HandleCreated(OrderEvent orderEvent)
        {
            await _serviceBus.Publish(new TransactionEvent
            {
                Order = orderEvent.Order,
                Transaction = Transaction.Create(orderEvent.Order.Reference),
                Type = TransactionEventType.Create,
            });
        }

        public async Task HandleProcess(OrderEvent orderEvent)
        {
            await _orderService.Process(orderEvent.Order, orderEvent.Transaction);
        }
    }
}
