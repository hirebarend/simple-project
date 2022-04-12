using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;

namespace SimpleProject.Application.Interfaces
{
    public interface IServiceBus
    {
        Task Publish(OrderEvent orderEvent);

        Task Publish(TransactionEvent transactionEvent);
    }
}
