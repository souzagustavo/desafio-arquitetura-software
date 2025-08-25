using MeuBolso.Domain.Account;
using MeuBolso.Domain.Transactions;
using MeuBolso.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeuBolso.Infrastructure.Common.Persistence;

public class MeuBolsoDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
    public DbSet<AccountBalanceEntity> AccountBalances { get; set; } = null!;
    public DbSet<TransactionEntity> Transactions { get; set; } = null!;
    public DbSet<AccountBalanceDay> AccountBalanceDays { get; set; } = null!;

    public MeuBolsoDbContext(DbContextOptions<MeuBolsoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuBolsoDbContext).Assembly);

        modelBuilder.UseStringEnums();

        modelBuilder.ConfigureIdentityTables();        
    }
}
