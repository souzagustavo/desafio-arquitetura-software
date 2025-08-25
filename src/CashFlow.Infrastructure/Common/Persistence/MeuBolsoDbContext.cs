using CashFlow.Domain.Account;
using CashFlow.Domain.Transactions;
using CashFlow.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Common.Persistence;

public class CashFlowDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
    public DbSet<AccountBalanceEntity> AccountBalances { get; set; } = null!;
    public DbSet<TransactionEntity> Transactions { get; set; } = null!;
    public DbSet<AccountBalanceDay> AccountBalanceDays { get; set; } = null!;

    public CashFlowDbContext(DbContextOptions<CashFlowDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CashFlowDbContext).Assembly);

        modelBuilder.UseStringEnums();

        modelBuilder.ConfigureIdentityTables();        
    }
}
