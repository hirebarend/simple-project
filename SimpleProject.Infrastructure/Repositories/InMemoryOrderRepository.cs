﻿using SimpleProject.Domain.Order;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;
using SimpleProject.Shared.Misc;

namespace SimpleProject.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        protected readonly object _lock = new object();

        public readonly IList<Order> _orders = new List<Order>();

        public Task<Order?> Find(string reference)
        {
            var order = _orders.FirstOrDefault(x => x.Reference == reference);

            return Task.FromResult(order);
        }

        public Task<Order> Insert(Order order)
        {
            lock (_lock)
            {
                ChaosMonkey.Do();

                var orderExisting = _orders.FirstOrDefault(x => x.Reference == order.Reference);

                if (orderExisting != null)
                {
                    return Task.FromResult(orderExisting);
                }

                _orders.Add(order);

                ChaosMonkey.Do();

                return Task.FromResult(order);
            }
        }

        public Task<Order> Update(Order order)
        {
            lock (_lock)
            {
                ChaosMonkey.Do();

                var orderExisting = _orders.FirstOrDefault(x => x.Reference == order.Reference);

                if (orderExisting == null)
                {
                    _orders.Add(order);

                    return Task.FromResult(order);

                    // throw new BusinessException($"unable to find order with reference, '{order.Reference}'");
                }

                if (orderExisting.Version != order.Version)
                {
                    return Task.FromResult(orderExisting);
                }

                orderExisting.Reference = order.Reference;
                orderExisting.State = order.State;
                orderExisting.Version = Convert.ToInt16(order.Version + 1);
                orderExisting.Updated = DateTimeOffset.UtcNow;

                ChaosMonkey.Do();

                return Task.FromResult(orderExisting);
            }
        }
    }
}
