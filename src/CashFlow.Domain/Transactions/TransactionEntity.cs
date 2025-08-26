using CashFlow.Application.Common;
using CashFlow.Domain.Store;

namespace CashFlow.Domain.Transactions
{
    public class TransactionEntity : BaseEntity
    {
        public Guid StoreId { get; set; }

        public DateTimeOffset? ProcessedAt { get; set; } = null;
        public required ETransactionType Type { get; set; }
        public required ETransactionStatus Status { get; set; } = ETransactionStatus.Pending;
        public required EPaymentMethod PaymentMethod { get; set; }
        public required decimal TotalAmount { get; set; } = 0;
        public string? Notes { get; set; }

        public virtual StoreEntity Store { get; set; } = null!;
    }
}
