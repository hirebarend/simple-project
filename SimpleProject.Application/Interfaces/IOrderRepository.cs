using SimpleProject.Domain.Entities;

namespace SimpleProject.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> Find(Account account, string reference);

        Task<Order> Insert(Account account, Order order);

        Task<Order> Update(Account account, Order order);
    }
}
