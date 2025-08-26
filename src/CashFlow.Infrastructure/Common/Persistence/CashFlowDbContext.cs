using CashFlow.Application.Common.Interfaces;
using CashFlow.Domain.Store;
using CashFlow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Common.Persistence;

public class CashFlowDbContext : DbContext, ICashFlowDbContext
{
    public DbSet<StoreEntity> Stores { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

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
