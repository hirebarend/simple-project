using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Order;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public class TimerFunction
    {
        protected readonly IServiceBus _serviceBus;

        public TimerFunction(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        [FunctionName("TimerFunction")]
        public async Task Run([TimerTrigger("0 */15 * * * *")]TimerInfo timerInfo, ILogger logger)
        {
            var orderEvent = new OrderEvent
            {
                Order = Order.Create(Guid.NewGuid().ToString()),
                Transaction = null,
                Type = OrderEventType.Create,
            };

            await _serviceBus.Publish(orderEvent);
        }
    }
}
