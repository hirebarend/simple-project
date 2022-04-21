using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;

namespace SimpleProject.Application.Interfaces
{
    public interface IDynamicRouteRepository
    {
        Task Insert(Account account, string reference, DynamicRouteResponse dynamicRouteResponse);

        Task Insert(Account account, string reference, DynamicRouteRequest dynamicRouteRequest);
    }
}
