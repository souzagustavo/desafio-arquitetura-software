using CashFlow.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Infrastructure.Account
{
    public class AccountBalanceDayEntityConfiguration : IEntityTypeConfiguration<AccountBalanceDay>
    {
        public void Configure(EntityTypeBuilder<AccountBalanceDay> builder)
        {
            builder.ToTable("AccountBalanceDay");
        }
    }
}
