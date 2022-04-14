using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SimpleProject.Application.EventHandlers;
using SimpleProject.Application.Interfaces;
using SimpleProject.Infrastructure.Gateways;
using SimpleProject.Infrastructure.Persistence.MongoDb;
using SimpleProject.Infrastructure.ServiceBuses;
using SimpleProject.Infrastructure.Services;

[assembly: FunctionsStartup(typeof(SimpleProject.FunctionApp.Startup))]
namespace SimpleProject.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            var mongoClientSettings = MongoClientSettings.FromConnectionString(configuration["MONGO_DB_CONNECTION_STRING"]);

            var mongoClient = new MongoClient(mongoClientSettings);

            var mongoDatabase = mongoClient.GetDatabase("simple-project");

            var mongoCollectionDynamicRoutes = mongoDatabase.GetCollection<Infrastructure.Persistence.MongoDb.DataTransferObjects.DynamicRoute>("objects");

            var mongoCollectionOrder = mongoDatabase.GetCollection<Infrastructure.Persistence.MongoDb.DataTransferObjects.Order>("orders");
            
            var mongoCollectionTransaction = mongoDatabase.GetCollection<Infrastructure.Persistence.MongoDb.DataTransferObjects.Transaction>("transactions");

            var serviceBusClient = new ServiceBusClient(configuration["AZURE_SERVICE_BUS_CONNECTION_STRING"]);

            builder.Services.AddSingleton(serviceBusClient);

            builder.Services.AddSingleton<IServiceBus, AzureServiceBus>();

            builder.Services.AddSingleton<IDynamicRouteRepository>(new MongoDbDynamicRouteRepository(mongoCollectionDynamicRoutes));

            builder.Services.AddSingleton<IOrderRepository>(x => new MongoDbOrderRepository(mongoCollectionOrder));

            builder.Services.AddSingleton<ITransactionRepository>(new MongoDbTransactionRepository(mongoCollectionTransaction));

            builder.Services.AddSingleton<OrderService>();

            builder.Services.AddSingleton<TransactionService>();

            builder.Services.AddSingleton<DynamicRoutingGateway>();

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

