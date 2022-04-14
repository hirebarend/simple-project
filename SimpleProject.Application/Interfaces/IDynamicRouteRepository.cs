using SimpleProject.Domain.ValueObjects;

namespace SimpleProject.Application.Interfaces
{
    public interface IDynamicRouteRepository
    {
        Task Insert(string reference, DynamicRouteResponse dynamicRouteResponse);

        Task Insert(string reference, DynamicRouteRequest dynamicRouteRequest);
    }
}
