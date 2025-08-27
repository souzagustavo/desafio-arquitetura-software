using CashFlow.Application.Common.Interfaces;
using CashFlow.Application.Transactions;
using CashFlow.Domain.Account;
using CashFlow.Infrastructure.Common.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RedLockNet;

namespace CashFlow.Infrastructure.Transaction
{
    public class TransactionLockProcessor : ITransactionLockProcessor
    {
        private const string LockKeyFormat = "lock-transaction-accountId-{1}";
        private const string LockOptionsKey = "TransactionAccount";

        private readonly RedLockOptions _redLockOptions;

        private readonly IDistributedLockFactory _lockFactory;
        private readonly ICashFlowDbContext _cashFlowDbContext;

        private readonly IAccountBalanceService _accountBalanceService;

        public TransactionLockProcessor(IDistributedLockFactory distributedLockFactory,
            ICashFlowDbContext cashFlowDbContext,
            IAccountBalanceService accountBalanceService,
            IOptions<RedLockRootOptions> options)
        {
            _lockFactory = distributedLockFactory;
            _cashFlowDbContext = cashFlowDbContext;
            _accountBalanceService = accountBalanceService;

            _redLockOptions = options.Value.GetOptions(LockOptionsKey);
        }

        public async Task DoAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            var transaction =
                await _cashFlowDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken)
                    ?? throw new InvalidOperationException($"Transaction with id {transactionId} not found.");

            var accountId = transaction.AccountId;

            await using var _lock =
                await _lockFactory.CreateLockAsync(GetLockKey(transaction.AccountId),
                    expiryTime: TimeSpan.FromSeconds(_redLockOptions.ExpiryTimeSeconds),
                    waitTime: TimeSpan.FromSeconds(_redLockOptions.WaitTimeSeconds),
                    retryTime: TimeSpan.FromSeconds(_redLockOptions.RetryTimeSeconds));

            if (_lock.IsAcquired)
            {
                var accountBalance =
                    await _cashFlowDbContext.AccountBalances.FirstOrDefaultAsync(ab => ab.AccountId == accountId, cancellationToken)
                        ?? throw new InvalidOperationException($"Account balance for account id {accountId} not found.");

                var isUpdated = _accountBalanceService.UpdateBalanceByTransaction(accountBalance, transaction);

                if (isUpdated)
                    transaction.MarkAsProcessed(accountBalance.CurrentTotal);
                else
                    transaction.MarkAsFailed();

                _cashFlowDbContext.AccountBalances.Update(accountBalance);
                _cashFlowDbContext.Transactions.Update(transaction);

                await _cashFlowDbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private static string GetLockKey(Guid accountId) => string.Format(LockKeyFormat, accountId);
    }
}
