using SimpleProject.Domain.Order;

namespace SimpleProject.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> Insert(Order order);

        Task<Order> Update(Order order);
    }
}
