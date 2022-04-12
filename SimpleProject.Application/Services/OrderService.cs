using SimpleProject.Application.Gateways;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Interfaces;

namespace SimpleProject.Application.Services
{
    public class OrderService
    {
        protected readonly IOrderRepository _orderRepository;

        protected readonly ProductGateway _productGateway;

        protected readonly IServiceBus _serviceBus;

        public OrderService(IOrderRepository orderRepository, ProductGateway productGateway, IServiceBus serviceBus)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

            _productGateway = productGateway ?? throw new ArgumentNullException(nameof(productGateway));

            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        public async Task Complete(Order order, Transaction transaction)
        {
            if (!order.Complete())
            {
                // TODO

                return;
            }

           order = await _orderRepository.Update(order);

            if (!order.IsCompleted())
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new OrderEvent
            {
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Completed,
            });
        }

        public async Task Create(Order order, Transaction transaction)
        {
            order = await _orderRepository.Insert(order);

            if (!order.IsPending())
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new OrderEvent
            {
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Created,
            });
        }

        public async Task Process(Order order, Transaction transaction)
        {
            if (!order.Process())
            {
                // TODO

                return;
            }

      
            order = await _orderRepository.Update(order);

            if (!order.IsProcessing())
            {
                // TODO

                return;
            }

            //await _serviceBus.Publish(new OrderEvent
            //{
            //    Order = order,
            //    Transaction = transaction,
            //    Type = OrderEventType.Processing,
            //});


            // TODO

            var result = await _productGateway.Purchase(order.Reference);

            if (!result)
            {
                //await _serviceBus.Publish(new OrderEvent
                //{
                //    Order = order,
                //    Transaction = transaction,
                //    Type = OrderEventType.Complete,
                //});

                return;
            }

            await _serviceBus.Publish(new OrderEvent
            {
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Processed,
            });
        }
    }
}
