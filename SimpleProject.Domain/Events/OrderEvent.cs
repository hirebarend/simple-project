using SimpleProject.Domain.Enums;

namespace SimpleProject.Domain.Events
{
    public class OrderEvent : Event
    {
        public OrderEventType Type { get; set; }
    }
}
