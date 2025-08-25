using CashFlow.Application.Common;

namespace CashFlow.Domain.Account
{
    public class AccountBalanceEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public decimal Total { get; set; } = 0;
        public virtual AccountEntity Account { get; set; } = null!;
    }
}
