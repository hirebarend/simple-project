using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SimpleProject.Application.EventHandlers;
using SimpleProject.Domain.Transaction;
using System;
using System.Threading.Tasks;

namespace SimpleProject.FunctionApp
{
    public class TransactionEventHandlerFunction
    {
        protected readonly TransactionEventHandler _transactionEventHandler;

        public TransactionEventHandlerFunction(TransactionEventHandler transactionEventHandler)
        {
            _transactionEventHandler = transactionEventHandler ?? throw new ArgumentNullException(nameof(transactionEventHandler));
        }

        [FunctionName("TransactionEventHandlerFunction")]
        public async Task Run([ServiceBusTrigger("transaction-events", Connection = "AZURE_SERVICE_BUS_CONNECTION_STRING")]TransactionEvent transactionEvent, ILogger log)
        {
            await _transactionEventHandler.Handle(transactionEvent);
        }
    }
}
