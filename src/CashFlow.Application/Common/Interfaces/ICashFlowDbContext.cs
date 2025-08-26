using CashFlow.Domain.Store;
using CashFlow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Application.Common.Interfaces;

public interface ICashFlowDbContext
{
    DbSet<StoreEntity> Stores { get; set; }
    DbSet<TransactionEntity> Transactions { get; set; }
    DbSet<StoreBalanceEntity> StoreBalances { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
