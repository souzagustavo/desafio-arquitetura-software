using CashFlow.Application.Common;
using CashFlow.Domain.Transactions;

namespace CashFlow.Domain.Store
{
    public class StoreEntity : BaseEntity
    {
        public Guid IdentityUserId { get; set; }
        public Guid StoreBalanceId { get; set; }

        public required string Name { get; set; }

        public virtual StoreBalanceEntity Balance { get; set; } = null!;
        public virtual ICollection<TransactionEntity> Transactions { get; set; } = [];
    }
}
