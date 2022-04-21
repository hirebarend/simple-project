using SimpleProject.Application.Interfaces;
using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;
using SimpleProject.Shared.Misc;
using System.Text.Json;

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

        public async Task<DynamicRouteResponse> Execute(Account account, string reference, DynamicRouteRequest dynamicRouteRequest)
        {
            try
            {
                await _dynamicRouteRepository.Insert(account, reference, dynamicRouteRequest);

                using (var httpClient = new HttpClient())
                {
                    var httpResponseMessage = dynamicRouteRequest.Method.Equals("GET") ? await httpClient.GetAsync(dynamicRouteRequest.Url) : await httpClient.PostAsync(dynamicRouteRequest.Url, new StringContent(JsonSerializer.Serialize(dynamicRouteRequest.Payload)));

                    var jsonPayload = await httpResponseMessage.Content.ReadAsStringAsync();

                    var payload = JsonSerializer.Deserialize<object?>(jsonPayload);

                    var dynamicRouteResponse = new DynamicRouteResponse
                    {
                        Payload = payload,
                        Success = httpResponseMessage.IsSuccessStatusCode,
                    };

                    await _dynamicRouteRepository.Insert(account, reference, dynamicRouteResponse);

                    return dynamicRouteResponse;
                }
            }
            catch
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
