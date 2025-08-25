﻿using CashFlow.Domain.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Infrastructure.Account
{
    public class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.HasMany(a => a.Transactions)
                   .WithOne(t => t.Account)
                   .HasForeignKey(t => t.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.DailyBalances)
                   .WithOne(t => t.Account)
                   .HasForeignKey(t => t.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Balance)
                   .WithOne(b => b.Account)
                   .HasForeignKey<AccountEntity>(a => a.AccountBalanceId);

            builder.ToTable("Account");
        }
    }
}
