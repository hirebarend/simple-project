using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;

namespace SimpleProject.Application.Interfaces
{
    public interface IDynamicRouteRepository
    {
        Task<DynamicRouteResponse?> FindResponse(Account account, string reference);

        Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest, DynamicRouteResponse dynamicRouteResponse);

        Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest);
    }
}
