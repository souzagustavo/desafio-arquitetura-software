using CashFlow.Domain.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Infrastructure.Store
{
    public class StoreBalanceEntityConfiguration : IEntityTypeConfiguration<StoreBalanceEntity>
    {
        public void Configure(EntityTypeBuilder<StoreBalanceEntity> builder)
        {
            builder.ToTable("StoreBalance");
        }
    }
}
