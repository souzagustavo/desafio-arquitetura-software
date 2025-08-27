using CashFlow.Domain.Transactions;

namespace CashFlow.Domain.Account
{
    public interface IAccountBalanceService
    {
        bool UpdateBalanceByTransaction(AccountBalanceEntity accountBalance,
            TransactionEntity transaction);
    }

    public class AccountBalanceService : IAccountBalanceService
    {
        public bool UpdateBalanceByTransaction(AccountBalanceEntity accountBalance,
            TransactionEntity transaction)
        {
            switch (transaction.Type)
            {
                case ETransactionType.Incoming:
                    accountBalance.CurrentTotal += transaction.TotalAmount;
                    return true;
                
                case ETransactionType.Outgoing:
                    // Por não se trata de uma conta corrente, não validamos se o saldo é suficiente.
                    accountBalance.CurrentTotal -= transaction.TotalAmount;
                    return true;

                default:
                    throw new NotImplementedException($"Transaction type {transaction.Type} not implemented.");
            }
        }
    }
}
