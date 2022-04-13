using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SimpleProject.Application.EventHandlers;
using SimpleProject.Application.Gateways;
using SimpleProject.Application.Interfaces;
using SimpleProject.Application.ServiceBuses;
using SimpleProject.Application.Services;
using SimpleProject.Infrastructure.InMemory;
using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Infrastructure.MongoDb;
using SimpleProject.Infrastructure.Repositories;

[assembly: FunctionsStartup(typeof(SimpleProject.FunctionApp.Startup))]
namespace SimpleProject.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            var mongoClient = new MongoClient(configuration["MONGO_DB_CONNECTION_STRING"]);

            var mongoDatabase = mongoClient.GetDatabase("simple-project");

            var mongoCollectionOrder = mongoDatabase.GetCollection<Infrastructure.MongoDb.DataTransferObjects.Order>("orders");

            var mongoCollectionTransaction = mongoDatabase.GetCollection<Infrastructure.MongoDb.DataTransferObjects.Transaction>("transactions");

            var serviceBusClient = new ServiceBusClient(configuration["AZURE_SERVICE_BUS_CONNECTION_STRING"]);

            builder.Services.AddSingleton(serviceBusClient);

            builder.Services.AddSingleton<IServiceBus, AzureServiceBus>();

            // builder.Services.AddSingleton<IOrderRepository>(x => new MongoDbOrderRepository(x.GetRequiredService<ILogger<MongoDbOrderRepository>>(), mongoCollectionOrder));

            builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

            builder.Services.AddSingleton<IProductGatewayLogRepository>(new InMemoryProductGatewayLogRepository());

            // builder.Services.AddSingleton<ITransactionRepository>(new MongoDbTransactionRepository(mongoCollectionTransaction));

            builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

            builder.Services.AddSingleton<OrderService>();

            builder.Services.AddSingleton<TransactionService>();

            builder.Services.AddSingleton<ProductGateway>();

            builder.Services.AddSingleton<OrderEventHandler>();

            builder.Services.AddSingleton<TransactionEventHandler>();

            //var logger = new LoggerConfiguration()
            //        .MinimumLevel.Information()
            //        .Filter.ByIncludingOnly(x =>
            //        {
            //            return x.Properties["SourceContext"].ToString().Contains("ProjectGecko") || x.Properties["SourceContext"].ToString().Contains("Function");
            //        })
            //        .WriteTo.Console()
            //        .WriteTo.File(new CompactJsonFormatter(), "log.txt")
            //        .CreateLogger();

            //builder.Services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddSerilog(logger);
            //});
        }
    }
}

