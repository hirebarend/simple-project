using SimpleProject.Domain.Entities;
using SimpleProject.Domain.Enums;

namespace SimpleProject.Domain.Events
{
    public class TransactionEvent : Event
    {
        public TransactionEventType Type { get; set; }
    }
}
