// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SimpleProject.Application.EventHandlers;
using SimpleProject.Application.Gateways;
using SimpleProject.Application.ServiceBuses;
using SimpleProject.Application.Services;
using SimpleProject.Domain.Order;
using SimpleProject.Domain.Transaction;
using SimpleProject.Infrastructure.MongoDb;
using SimpleProject.Infrastructure.Repositories;
using SimpleProject.Shared.Exceptions;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));

var mongoDatabase = mongoClient.GetDatabase("simple-project");

var mongoCollection = mongoDatabase.GetCollection<SimpleProject.Infrastructure.MongoDb.DataTransferObjects.Order>("orders");

var serviceBusClient = new ServiceBusClient(configuration.GetConnectionString("AzureServiceBus"));

var orderRepository = new MongoDbOrderRepository(mongoCollection);

//var orderRepository = new MsSqlServerOrderRepository(configuration.GetConnectionString("MsSqlServer"));

//orderRepository.DeleteAll().GetAwaiter().GetResult();

var productGatewayLogRepository = new MsSqlServerProductGatewayLogRepository(configuration.GetConnectionString("MsSqlServer"));

productGatewayLogRepository.DeleteAll().GetAwaiter().GetResult();

var transactionRepository = new MsSqlServerTransactionRepository(configuration.GetConnectionString("MsSqlServer"));

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
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        throw;
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
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        throw;
    }
}

serviceBusProcessorTransactionEvents.StartProcessingAsync();

//for (var i = 0; i < 100; i++)
//{
//    serviceBus.Publish(new OrderEvent
//    {
//        Order = Order.Create(Guid.NewGuid().ToString()),
//        Transaction = null,
//        Type = OrderEventType.Create,
//    }).GetAwaiter().GetResult();
//}

serviceBus.Publish(new OrderEvent
{
    Order = Order.Create(Guid.NewGuid().ToString()),
    Transaction = null,
    Type = OrderEventType.Create,
}).GetAwaiter().GetResult();

Console.ReadKey();
