using SimpleProject.Domain.Entities;

namespace SimpleProject.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> Find(string reference);

        Task<Order> Insert(Order order);

        Task<Order> Update(Order order);
    }
}
