using CashFlow.Domain.Account;
using Microsoft.AspNetCore.Identity;

namespace CashFlow.Domain.User
{
    public class UserEntity : IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public string? DocumentNumber { get; set; }

        public virtual ICollection<AccountEntity> Accounts { get; set; } = [];
    }
}
