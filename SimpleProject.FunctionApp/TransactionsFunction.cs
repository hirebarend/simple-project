using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public class TransactionsFunction
    {
        protected readonly ITransactionRepository _transactionRepository;

        protected readonly IServiceBus _serviceBus;

        public TransactionsFunction(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        [FunctionName("Transactions")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "Transactions/{reference}")] HttpRequest httpRequest,
            string reference,
            ILogger logger)
        {
            var transaction = await _transactionRepository.Find(null, reference);

            return new OkObjectResult(transaction);
        }
    }
}
