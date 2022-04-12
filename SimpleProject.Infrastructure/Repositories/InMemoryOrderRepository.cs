using SimpleProject.Domain.Order;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Exceptions;
using SimpleProject.Shared.Misc;

namespace SimpleProject.Infrastructure.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        protected object _lock = new object();

        public readonly IList<Order> _orders = new List<Order>();

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

                order.Created = DateTimeOffset.UtcNow;
                order.Updated = DateTimeOffset.UtcNow;

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
                    throw new BusinessException($"unable to find order with reference, '{order.Reference}'");
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
