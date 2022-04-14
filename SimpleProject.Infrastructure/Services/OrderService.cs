using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Enums;
using SimpleProject.Domain.Events;
using SimpleProject.Domain.ValueObjects;
using SimpleProject.Infrastructure.Gateways;

namespace SimpleProject.Infrastructure.Services
{
    public class OrderService
    {
        protected readonly IOrderRepository _orderRepository;

        protected readonly DynamicRoutingGateway _dynamicRoutingGatewayResponse;

        protected readonly IServiceBus _serviceBus;

        public OrderService(DynamicRoutingGateway dynamicRoutingGatewayResponse, IOrderRepository orderRepository, IServiceBus serviceBus)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

            _dynamicRoutingGatewayResponse = dynamicRoutingGatewayResponse ?? throw new ArgumentNullException(nameof(dynamicRoutingGatewayResponse));

            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        public async Task Cancel(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
        {
            if (!order.Cancel())
            {
                // TODO

                return;
            }

            order = await _orderRepository.Update(order);

            if (!order.IsCancelled())
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new OrderEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Cancelled,
            });
        }

        public async Task Complete(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
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
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Completed,
            });
        }

        public async Task Create(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
        {
            order = await _orderRepository.Insert(order);

            if (!order.IsPending())
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new OrderEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Created,
            });
        }

        public async Task Process(Account account, DynamicRouteRequest dynamicRouteRequest, Order order, Transaction transaction)
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

            await _serviceBus.Publish(new OrderEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Processing,
            });

            var dynamicRouteResponse = await _dynamicRoutingGatewayResponse.Execute(order.Reference, dynamicRouteRequest);

            if (!order.Processed())
            {
                // TODO

                return;
            }

            order = await _orderRepository.Update(order);

            if (!order.IsProcessed())
            {
                // TODO

                return;
            }

            await _serviceBus.Publish(new OrderEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Processed,
            });


            if (!dynamicRouteResponse.Success)
            {
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

            await _serviceBus.Publish(new OrderEvent
            {
                Account = account,
                DynamicRouteRequest = dynamicRouteRequest,
                Order = order,
                Transaction = transaction,
                Type = OrderEventType.Complete,
            });
        }
    }
}
