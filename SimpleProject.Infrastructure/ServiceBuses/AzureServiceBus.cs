using Azure.Messaging.ServiceBus;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Events;

namespace SimpleProject.Infrastructure.ServiceBuses
{
    public class AzureServiceBus : IServiceBus
    {
        protected readonly ServiceBusClient _serviceBusClient;

        protected readonly ServiceBusSender _serviceBusSenderOrderEvents;

        protected readonly ServiceBusSender _serviceBusSenderTransactionEvents;

        public AzureServiceBus(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient ?? throw new ArgumentNullException(nameof(serviceBusClient));

            _serviceBusSenderOrderEvents = serviceBusClient.CreateSender("order-events");

            _serviceBusSenderTransactionEvents = serviceBusClient.CreateSender("transaction-events");
        }

        public async Task Publish(OrderEvent orderEvent)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(orderEvent);

            var serviceBusMessage = new ServiceBusMessage(json);

            await _serviceBusSenderOrderEvents.SendMessageAsync(serviceBusMessage);
        }

        public async Task Publish(TransactionEvent transactionEvent)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(transactionEvent);

            var serviceBusMessage = new ServiceBusMessage(json);

            await _serviceBusSenderTransactionEvents.SendMessageAsync(serviceBusMessage);
        }
    }
}
