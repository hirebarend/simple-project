
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

            if (orderEvent.Type == OrderEventType.Complete)
            {
                await _orderService.Complete(orderEvent.Order, orderEvent.Transaction);

                return;
            }

            // Step 1
            if (orderEvent.Type == OrderEventType.Create)
            {
                await _orderService.Create(orderEvent.Order, orderEvent.Transaction);

                return;
            }

            // Step 2
            if (orderEvent.Type == OrderEventType.Created)
            {
                await _serviceBus.Publish(new TransactionEvent
                {
                    Order = orderEvent.Order,
                    Transaction = Transaction.Create(orderEvent.Order.Reference),
                    Type = TransactionEventType.Create,
                });

                return;
            }

            // Step 7
            if (orderEvent.Type == OrderEventType.Process)
            {
                await _orderService.Process(orderEvent.Order, orderEvent.Transaction);

                return;
            }

            // Step 8
            if (orderEvent.Type == OrderEventType.Processed)
            {
                await _serviceBus.Publish(new TransactionEvent
                {
                    Order = orderEvent.Order,
                    Transaction = orderEvent.Transaction,
                    Type = TransactionEventType.Settle,
                });

                return;
            }

            // Step 11
            if (orderEvent.Type == OrderEventType.Complete)
            {
                await _orderService.Complete(orderEvent.Order, orderEvent.Transaction);

                return;
            }

            // Step 12
            if (orderEvent.Type == OrderEventType.Completed)
            {
                Console.WriteLine($"[Order] {orderEvent.Order.State} {orderEvent.Order.Updated.Subtract(orderEvent.Order.Created).TotalMilliseconds}");

                return;
            }

            if (orderEvent.Type == OrderEventType.Cancel)
            {
                await _orderService.Cancel(orderEvent.Order, orderEvent.Transaction);

                return;
            }

            if (orderEvent.Type == OrderEventType.Cancelled)
            {
                await _serviceBus.Publish(new TransactionEvent
                {
                    Order = orderEvent.Order,
                    Transaction = orderEvent.Transaction,
                    Type = TransactionEventType.Void,
                });

                return;
            }
        }
    }
}
