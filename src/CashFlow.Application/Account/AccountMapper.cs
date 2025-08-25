using CashFlow.Application.Account.Handlers;
using CashFlow.Domain.Account;
using Riok.Mapperly.Abstractions;

namespace CashFlow.Application.Account
{
    [Mapper]
    public partial class AccountMapper
    {
        public partial AccountEntity ToAccountEntity(CreateAccountRequest accountRequest);
        public partial GetAccountResponse ToGetAccountResponse(AccountEntity accountEntity);
    }
}
