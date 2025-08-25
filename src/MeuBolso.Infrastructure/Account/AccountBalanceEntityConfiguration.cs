using MeuBolso.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuBolso.Infrastructure.Account
{
    public class AccountBalanceEntityConfiguration : IEntityTypeConfiguration<AccountBalanceEntity>
    {
        public void Configure(EntityTypeBuilder<AccountBalanceEntity> builder)
        {
            builder.ToTable("AccountBalance");
        }
    }
}
