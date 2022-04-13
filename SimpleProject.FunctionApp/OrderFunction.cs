using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Order;
using SimpleProject.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public class OrderFunction
    {
        protected readonly IOrderRepository _orderRepository;

        protected readonly IServiceBus _serviceBus;

        public OrderFunction(IOrderRepository orderRepository, IServiceBus serviceBus)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));

            _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        }

        [FunctionName("Order")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "POST", Route = "Order/{reference}")] HttpRequest httpRequest,
            string reference,
            ILogger logger)
        {
            if (httpRequest.Method.Equals("GET", StringComparison.InvariantCultureIgnoreCase))
            {
                var order = await _orderRepository.Find(reference);

                return new OkObjectResult(order);
            }

            if (httpRequest.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                var orderEvent = new OrderEvent
                {
                    Order = Order.Create(reference),
                    Transaction = null,
                    Type = OrderEventType.Create,
                };

                await _serviceBus.Publish(orderEvent);

                return new OkObjectResult(orderEvent);
            }

            return new OkObjectResult(true);
        }
    }
}
