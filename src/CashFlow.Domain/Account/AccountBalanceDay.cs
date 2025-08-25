using CashFlow.Application.Common;

namespace CashFlow.Domain.Account
{
    public class AccountBalanceDay : BaseEntity
    {
        public Guid AccountId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Total { get; set; }
        public virtual AccountEntity Account { get; set; } = null!;
    }
}
