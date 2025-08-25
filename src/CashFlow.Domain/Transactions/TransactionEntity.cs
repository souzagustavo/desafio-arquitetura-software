using CashFlow.Application.Common;
using CashFlow.Application.Transactions;
using CashFlow.Domain.Store;

namespace CashFlow.Domain.Transactions
{
    public class TransactionEntity : BaseEntity
    {
        public Guid StoreId { get; set; }

        public DateTimeOffset OccurrentAt { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; } = null;
        public ETransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public virtual StoreEntity Store { get; set; } = null!;
    }
}
