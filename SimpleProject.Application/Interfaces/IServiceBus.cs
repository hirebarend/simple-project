using SimpleProject.Domain.Events;

namespace SimpleProject.Application.Interfaces
{
    public interface IServiceBus
    {
        Task Publish(OrderEvent orderEvent);

        Task Publish(TransactionEvent transactionEvent);
    }
}
