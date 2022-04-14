using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.EventHandlers;
using SimpleProject.Domain.Events;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public class OrderEventHandlerFunction
    {
        protected readonly OrderEventHandler _orderEventHandler;

        public OrderEventHandlerFunction(OrderEventHandler orderEventHandler)
        {
            _orderEventHandler = orderEventHandler ?? throw new ArgumentNullException(nameof(orderEventHandler));
        }

        [FunctionName("OrderEventHandlerFunction")]
        public async Task Run([ServiceBusTrigger("order-events", Connection = "AZURE_SERVICE_BUS_CONNECTION_STRING")]OrderEvent orderEvent, ILogger log)
        {
            await _orderEventHandler.Handle(orderEvent);
        }
    }
}
