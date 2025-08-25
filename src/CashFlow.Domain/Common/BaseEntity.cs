namespace CashFlow.Application.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private init; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; } = null;

        protected BaseEntity()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
