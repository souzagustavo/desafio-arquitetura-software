using CashFlow.Application.Common;

namespace CashFlow.Domain.Account
{
    public class AccountBalanceEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public decimal CurrentTotal { get; set; } = 0;
        public virtual AccountEntity Account { get; set; } = null!;
    }
}
