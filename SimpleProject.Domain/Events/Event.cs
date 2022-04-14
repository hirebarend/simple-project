using SimpleProject.Domain.Entities;
using SimpleProject.Domain.ValueObjects;

namespace SimpleProject.Domain.Events
{
    public class Event
    {
        public Account Account { get; set; }

        public DynamicRouteRequest DynamicRouteRequest { get; set; }

        public Order Order { get; set; }

         public Transaction Transaction { get; set; }
    }
}
