using CashFlow.Application.Common;

namespace CashFlow.Domain.Store
{
    public class StoreBalanceEntity : BaseEntity
    {
        public Guid StoreId { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public virtual StoreEntity Store { get; set; } = null!;
    }
}
