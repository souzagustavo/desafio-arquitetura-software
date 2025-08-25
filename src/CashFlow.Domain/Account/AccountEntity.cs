using CashFlow.Application.Common;
using CashFlow.Domain.Transactions;
using CashFlow.Domain.User;

namespace CashFlow.Domain.Account
{
    public class AccountEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AccountBalanceId { get; set; }
        public EAccountType Type { get; set; }
        
        public virtual UserEntity User { get; set; } = null!;

        public virtual AccountBalanceEntity Balance { get; set; } = null!;
        public virtual ICollection<TransactionEntity> Transactions { get; set; } = [];
        public virtual ICollection<AccountBalanceDay> DailyBalances { get; set; } = [];
    }
}
