using CashFlow.Domain.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Infrastructure.Store
{
    public class StoreEntityConfiguration : IEntityTypeConfiguration<StoreEntity>
    {
        public void Configure(EntityTypeBuilder<StoreEntity> builder)
        {
            builder.HasMany(a => a.Transactions)
                   .WithOne(t => t.Store)
                   .HasForeignKey(t => t.StoreId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.DailyBalances)
                   .WithOne(t => t.Store)
                   .HasForeignKey(t => t.StoreId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Balance)
                   .WithOne(b => b.Store)
                   .HasForeignKey<StoreEntity>(a => a.StoreBalanceId);

            builder.ToTable("Store");
        }
    }
}
