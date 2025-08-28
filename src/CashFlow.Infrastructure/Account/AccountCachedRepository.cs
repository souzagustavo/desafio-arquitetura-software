using CashFlow.Application.Account;
using CashFlow.Application.Account.Handlers;
using CashFlow.Application.AccountBalance.Handlers;
using CashFlow.Application.Common.Interfaces;
using CashFlow.Domain.Account;
using CashFlow.Domain.Transactions;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json;

namespace CashFlow.Infrastructure.Account;

public class AccountCachedRepository : IAccountCachedRepository
{
    private const string AccountsCacheKeyFormat = "accounts-userId:{0}";
    private const string AccountBalanceCacheKeyFormat = "balance-accountId:{0}";
    private const string AccountDailyBalanceCacheKeyFormat = "daily-balance:{0}:accountId:{1}";

    private const int DefaultCacheDurationInMinutes = 30;
    private const int DefaultLocalCacheDurationInSeconds = 15;

    private readonly HybridCache _hybridCache;
    private readonly ICashFlowDbContext _cashFlowDbContext;

    private readonly AccountMapper _accountMapper = new();

    public AccountCachedRepository(HybridCache hybridCache, ICashFlowDbContext cashFlowDbContext)
    {
        _hybridCache = hybridCache;
        _cashFlowDbContext = cashFlowDbContext;
    }

    public async Task CreateAccountAsync(AccountEntity accountEntity, CancellationToken cancellationToken)
    {
        await _cashFlowDbContext.Accounts.AddAsync(accountEntity, cancellationToken);
        var accountBalanceEntity = new AccountBalanceEntity()
        {
            AccountId = accountEntity.Id,
            Account = accountEntity,
            CurrentTotal = 0,
        };
        await _cashFlowDbContext.AccountBalances.AddAsync(accountBalanceEntity, cancellationToken);
        await _cashFlowDbContext.SaveChangesAsync(cancellationToken);

        var accounts = (await _cashFlowDbContext.Accounts
                .Where(a => a.IdentityUserId == accountEntity.IdentityUserId)
                .ToListAsync(cancellationToken))
                .Select(x => _accountMapper.ToGetAccountResponse(x));
        var userAccountsKey = string.Format(AccountsCacheKeyFormat, accountEntity.IdentityUserId);
        await SetToCacheAsync(userAccountsKey, accounts, cancellationToken);

        var accountBalanceResponse = _accountMapper.ToGetAccountBalanceResponse(accountBalanceEntity);
        var balanceAccountKey = string.Format(AccountBalanceCacheKeyFormat, accountEntity.Id);
        await SetToCacheAsync(balanceAccountKey, accountBalanceResponse, cancellationToken);
    }

    public async Task UpdateBalancesAndTransacionAsync(AccountBalanceEntity balance,
        AccountDailyBalanceEntity dailyBalance, TransactionEntity transaction, CancellationToken cancellationToken)
    {
        _cashFlowDbContext.AccountBalances.Update(balance);
        _cashFlowDbContext.AccountDailyBalance.Update(dailyBalance);
        _cashFlowDbContext.Transactions.Update(transaction);
        await _cashFlowDbContext.SaveChangesAsync(cancellationToken);

        var balanceKey = string.Format(AccountBalanceCacheKeyFormat, balance.AccountId);
        var accountBalanceResponse = _accountMapper.ToGetAccountBalanceResponse(balance);
        await SetToCacheAsync(balanceKey, accountBalanceResponse, cancellationToken);

        var dailyBalanceKey = string.Format(AccountDailyBalanceCacheKeyFormat, dailyBalance.DateToString(), dailyBalance.AccountId);
        var dailyBalanceResponse = _accountMapper.ToGetDailyBalanceResponse(dailyBalance);
        await SetToCacheAsync(dailyBalanceKey, dailyBalanceResponse, cancellationToken);
    }

    public async Task<GetAccountBalanceResponse?> GetBalanceAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var key = string.Format(AccountBalanceCacheKeyFormat, accountId);

        return await _hybridCache.GetOrCreateAsync(key, async token =>
        {
            var accountBalanceEntity =
                await _cashFlowDbContext.AccountBalances
                    .Where(b => b.AccountId == accountId)
                    .FirstOrDefaultAsync(cancellationToken);

            if (accountBalanceEntity is null)
                return null;

            return _accountMapper.ToGetAccountBalanceResponse(accountBalanceEntity);
        },
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(DefaultCacheDurationInMinutes),
            LocalCacheExpiration = TimeSpan.FromSeconds(DefaultLocalCacheDurationInSeconds)
        },
        cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<GetAccountResponse>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var key = string.Format(AccountsCacheKeyFormat, userId);

        return await _hybridCache.GetOrCreateAsync(key, async token =>
        {
            IEnumerable<GetAccountResponse> accounts =
                (await _cashFlowDbContext.Accounts
                .Where(b => b.IdentityUserId == userId)
                .ToListAsync(cancellationToken))
                .Select(a => _accountMapper.ToGetAccountResponse(a));

            return accounts;
        },
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(DefaultCacheDurationInMinutes),
            LocalCacheExpiration = TimeSpan.FromSeconds(DefaultLocalCacheDurationInSeconds)
        },
        cancellationToken: cancellationToken);
    }

    public async Task<AccountDailyBalanceEntity> GetOrCreateDailyBalanceAsync(Guid accountId, DateOnly date)
    {
        var dailyBalance =
            await _cashFlowDbContext.AccountDailyBalance.FirstOrDefaultAsync(x => x.Id == accountId && x.Date == date);

        dailyBalance ??= new AccountDailyBalanceEntity()
            {
                AccountId = accountId,
                Date = date
            };

        await _cashFlowDbContext.AccountDailyBalance.AddAsync(dailyBalance);

        return dailyBalance;
    }

    private static HybridCacheEntryOptions GetDefaultCacheOptions()
        => new()
        {
            Expiration = TimeSpan.FromMinutes(DefaultCacheDurationInMinutes),
            LocalCacheExpiration = TimeSpan.FromSeconds(DefaultLocalCacheDurationInSeconds)
        };

    private async Task SetToCacheAsync<T>(string key, T @object, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(@object);
        await _hybridCache.SetAsync(key, json, options: GetDefaultCacheOptions(), tags: null, cancellationToken);
    }

}