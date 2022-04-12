namespace SimpleProject.Infrastructure.Interfaces
{
    public interface IProductGatewayLogRepository
    {
        Task Insert(string reference);
    }
}
