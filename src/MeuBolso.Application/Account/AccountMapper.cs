using MeuBolso.Application.Account.Handlers;
using MeuBolso.Domain.Account;
using Riok.Mapperly.Abstractions;

namespace MeuBolso.Application.Account
{
    [Mapper]
    public partial class AccountMapper
    {
        public partial AccountEntity ToAccountEntity(CreateAccountRequest accountRequest);
        public partial GetAccountResponse ToGetAccountResponse(AccountEntity accountEntity);
    }
}
