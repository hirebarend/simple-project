// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using SimpleProject.Application.EventHandlers;
using SimpleProject.Application.Gateways;
using SimpleProject.Application.ServiceBuses;
using SimpleProject.Application.Services;
using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.Repositories;
using SimpleProject.Shared.Exceptions;

Console.WriteLine("Hello, World!");

var connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=ProjectGecko; Integrated Security=True;";

var serviceBusClient = new ServiceBusClient("Endpoint=sb://simple-project.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=YF/5618v6HAvujlxQ6ZofvOZn36XXBKb/TDPbpiSvfg=");

// var orderRepository = new InMemoryOrderRepository();

var orderRepository = new MsSqlServerOrderRepository(connectionString);

orderRepository.DeleteAll().GetAwaiter().GetResult();

var productGatewayLogRepository = new MsSqlServerProductGatewayLogRepository(connectionString);

productGatewayLogRepository.DeleteAll().GetAwaiter().GetResult();

var transactionRepository = new InMemoryTransactionRepository();

var productGateway = new ProductGateway(productGatewayLogRepository);

var serviceBus = new AzureServiceBus(serviceBusClient);

var orderService = new OrderService(orderRepository, productGateway, serviceBus);

var transactionService = new TransactionService(serviceBus, transactionRepository);

var orderEventHandler = new OrderEventHandler(orderService, serviceBus, transactionService);

var transactionEventHandler = new TransactionEventHandler(orderService, serviceBus, transactionService);

var serviceBusProcessorOrderEvents = serviceBusClient.CreateProcessor("order-events");

serviceBusProcessorOrderEvents.ProcessErrorAsync += ServiceBusProcessorOrderEvents_ProcessErrorAsync;

Task ServiceBusProcessorOrderEvents_ProcessErrorAsync(ProcessErrorEventArgs arg)
{
    return Task.CompletedTask;
}

serviceBusProcessorOrderEvents.ProcessMessageAsync += ServiceBusProcessorOrderEvents_ProcessMessageAsync;

async Task ServiceBusProcessorOrderEvents_ProcessMessageAsync(ProcessMessageEventArgs arg)
{
    try
    {
        var orderEvent = arg.Message.Body.ToObjectFromJson<OrderEvent>();

        await orderEventHandler.Handle(orderEvent);
    }
    catch (BusinessException ex)
    {
        Console.WriteLine(ex.Message);

        return;
    }
}

serviceBusProcessorOrderEvents.StartProcessingAsync();

var serviceBusProcessorTransactionEvents = serviceBusClient.CreateProcessor("transaction-events");

serviceBusProcessorTransactionEvents.ProcessErrorAsync += ServiceBusProcessorTransactionEvents_ProcessErrorAsync;

Task ServiceBusProcessorTransactionEvents_ProcessErrorAsync(ProcessErrorEventArgs arg)
{
    return Task.CompletedTask;
}

serviceBusProcessorTransactionEvents.ProcessMessageAsync += ServiceBusProcessorTransactionEvents_ProcessMessageAsync;

async Task ServiceBusProcessorTransactionEvents_ProcessMessageAsync(ProcessMessageEventArgs arg)
{
    try
    {
        var transactionEvent = arg.Message.Body.ToObjectFromJson<TransactionEvent>();

        await transactionEventHandler.Handle(transactionEvent);
    }
    catch (BusinessException ex)
    {
        Console.WriteLine(ex.Message);

        return;
    }
}

serviceBusProcessorTransactionEvents.StartProcessingAsync();

//var count = 0;

//while (count < 150)
//{
//    try
//    {
//        serviceBus.Publish(new OrderEvent
//        {
//            Order = Order.Create(Guid.NewGuid().ToString()),
//            Transaction = null,
//            Type = OrderEventType.Create,
//        }).GetAwaiter().GetResult();

//        count++;
//    }
//    catch
//    {
//    }
//}

serviceBus.Publish(new OrderEvent
{
    Order = Order.Create(Guid.NewGuid().ToString()),
    Transaction = null,
    Type = OrderEventType.Create,
}).GetAwaiter().GetResult();


//while (true)
//{
//    Console.WriteLine($"{orderRepository._orders.Count(x => x.State == OrderState.Completed)} / {orderRepository._orders.Count()}");

//    Thread.Sleep(1000);
//}

Console.ReadKey();