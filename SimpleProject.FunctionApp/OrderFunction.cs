using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Order;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public class OrderFunction
    {
        protected readonly IServiceBus _serviceBus;

        public OrderFunction(IServiceBus serviceBus)
        {
            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        [FunctionName("Order")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest httpRequest,
            ILogger logger)
        {
            var orderEvent = new OrderEvent
            {
                Order = Order.Create(Guid.NewGuid().ToString()),
                Transaction = null,
                Type = OrderEventType.Create,
            };

            await _serviceBus.Publish(orderEvent);

            return new OkObjectResult(orderEvent);
        }
    }
}
