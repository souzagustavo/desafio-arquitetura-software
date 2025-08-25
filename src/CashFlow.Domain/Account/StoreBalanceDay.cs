using CashFlow.Application.Common;

namespace CashFlow.Domain.Store
{
    public class StoreBalanceDay : BaseEntity
    {
        public Guid StoreId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Total { get; set; }
        public virtual StoreEntity Store { get; set; } = null!;
    }
}
