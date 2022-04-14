using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.ValueObjects;
using SimpleProject.Shared.Misc;

namespace SimpleProject.Infrastructure.Gateways
{
    public class DynamicRoutingGateway
    {
        protected readonly CircuitBreaker _circuitBreaker = new CircuitBreaker();

        protected readonly IDynamicRouteRepository _dynamicRouteRepository;

        public DynamicRoutingGateway(IDynamicRouteRepository dynamicRouteRepository)
        {
            _dynamicRouteRepository = dynamicRouteRepository ?? throw new ArgumentNullException(nameof(dynamicRouteRepository));
        }

        public async Task<DynamicRouteResponse> Execute(string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            try
            {
                // await _dynamicRouteRepository.Insert(reference, dynamicRouteRequest);

                using (var httpClient = new HttpClient())
                {
                    var httpResponseMessage = await httpClient.GetAsync(dynamicRouteRequest.Url);

                    var jsonPayload = await httpResponseMessage.Content.ReadAsStringAsync();

                    var payload = System.Text.Json.JsonSerializer.Deserialize<object?>(jsonPayload);

                    var dynamicRouteResponse = new DynamicRouteResponse
                    {
                        Payload = payload,
                        Success = httpResponseMessage.IsSuccessStatusCode,
                    };

                    await _dynamicRouteRepository.Insert(reference, dynamicRouteResponse);

                    return dynamicRouteResponse;
                }
            }
            catch(Exception exception)
            {
                return new DynamicRouteResponse
                {
                    Payload = null,
                    Success = false,
                };
            }
        }
    }
}
