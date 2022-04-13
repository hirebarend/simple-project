using SimpleProject.Infrastructure.Interfaces;

namespace SimpleProject.Infrastructure.InMemory
{
    public class InMemoryProductGatewayLogRepository : IProductGatewayLogRepository
    {
        public Task Insert(string reference)
        {
            return Task.CompletedTask;
        }
    }
}
