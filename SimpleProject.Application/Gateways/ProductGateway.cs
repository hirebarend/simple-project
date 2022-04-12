using SimpleProject.Infrastructure.Interfaces;
using SimpleProject.Shared.Misc;

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
            try
            {
                await _productGatewayLogRepository.Insert(reference);

                ChaosMonkey.Do();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
