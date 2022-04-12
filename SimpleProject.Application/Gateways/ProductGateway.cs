using SimpleProject.Infrastructure.Interfaces;

namespace SimpleProject.Application.Gateways
{
    public class ProductGateway
    {
        protected readonly IProductGatewayLogRepository _productGatewayLogRepository;

        public ProductGateway(IProductGatewayLogRepository productGatewayLogRepository)
        {
            _productGatewayLogRepository = productGatewayLogRepository ?? throw new ArgumentNullException(nameof(productGatewayLogRepository));
        }
    
        public async Task<bool> Purchase(string reference)
        {
            await _productGatewayLogRepository.Insert(reference);

            return true;
        }
    }
}
