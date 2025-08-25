using CashFlow.Domain.Store;
using CashFlow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Common.Persistence;

public class CashFlowDbContext : DbContext
{
    public DbSet<StoreEntity> Stores { get; set; } = null!;
    public DbSet<StoreBalanceEntity> StoreBalances { get; set; } = null!;
    public DbSet<TransactionEntity> Transactions { get; set; } = null!;
    public DbSet<StoreBalanceDay> StoreBalanceDays { get; set; } = null!;

    public CashFlowDbContext(DbContextOptions<CashFlowDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashFlowDbContext).Assembly);

        modelBuilder.UseStringEnums();
    }
}
