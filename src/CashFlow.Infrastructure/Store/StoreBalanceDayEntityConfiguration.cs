using CashFlow.Domain.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Infrastructure.Store
{
    public class StoreBalanceDayEntityConfiguration : IEntityTypeConfiguration<StoreBalanceDay>
    {
        public void Configure(EntityTypeBuilder<StoreBalanceDay> builder)
        {
            builder.ToTable("StoreBalanceDay");
        }
    }
}
