using CashFlow.Domain.Account;
using CashFlow.Domain.Transactions;

namespace CashFlow.Application.Account
{
    public interface IAccountService
    {
        Task UpdateBalancesByTransactionAsync(AccountBalanceEntity accountBalance,
            TransactionEntity transaction, CancellationToken cancellationToken);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountCachedRepository _accountCachedRepository;

        public AccountService(IAccountCachedRepository accountCachedRepository)
        {
            _accountCachedRepository = accountCachedRepository;
        }

        public async Task UpdateBalancesByTransactionAsync(
            AccountBalanceEntity accountBalance,
            TransactionEntity transaction,
            CancellationToken cancellationToken)
        {
            var accountDailyBalance =
                await _accountCachedRepository
                        .GetOrCreateDailyBalanceAsync(accountBalance.AccountId, transaction.GetDateCreated());
            
            var totalAmountTransaction = transaction.TotalAmount;

            switch (transaction.Type)
            {
                case ETransactionType.Incoming:
                    accountBalance.CurrentTotal += totalAmountTransaction;
                    accountDailyBalance.TotalIncoming += totalAmountTransaction;
                    break;

                case ETransactionType.Outgoing:
                    accountBalance.CurrentTotal -= totalAmountTransaction;
                    accountDailyBalance.TotalOutgoing += totalAmountTransaction;
                    break;
                default:
                    throw new NotImplementedException($"Transaction type {transaction.Type} not implemented.");
            }
            transaction.MarkAsProcessed();

            await _accountCachedRepository
                .UpdateBalancesAndTransacionAsync(accountBalance, accountDailyBalance, transaction, cancellationToken);
        }
    }
}
