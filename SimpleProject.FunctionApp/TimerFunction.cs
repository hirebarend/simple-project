using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Enums;
using SimpleProject.Domain.Events;
using SimpleProject.Domain.ValueObjects;
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
            var reference = Guid.NewGuid().ToString();

            var orderEvent = new OrderEvent
            {
                Account = new Account
                {
                      Reference = Guid.NewGuid().ToString(),
                },
                DynamicRouteRequest = new DynamicRouteRequest
                {
                    Method = "GET",
                    Payload = null,
                    Url = "http://data.fixer.io/api/latest?access_key=eadd3f04a3179173fe19955aeac8fb01"
                },
                Order = Order.Create(reference),
                Transaction = Transaction.Create(10, reference),
                Type = OrderEventType.Create,
            };

            await _serviceBus.Publish(orderEvent);
        }
    }
}
