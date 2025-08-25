using MeuBolso.Application.Common;
using MeuBolso.Application.Transactions;
using MeuBolso.Domain.Account;

namespace MeuBolso.Domain.Transactions
{
    public class TransactionEntity : BaseEntity
    {
        public Guid AccountId { get; set; }

        public DateTimeOffset OccurrentAt { get; set; }
        public DateTimeOffset? ProcessedAt { get; set; } = null;
        public ETransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }

        public virtual AccountEntity Account { get; set; } = null!;
    }
}
