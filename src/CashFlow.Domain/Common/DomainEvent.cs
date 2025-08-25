namespace CashFlow.Domain.Common
{
    public abstract class DomainEvent
    {
        public DateTime Timestamp { get; set; }

        protected DomainEvent()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
