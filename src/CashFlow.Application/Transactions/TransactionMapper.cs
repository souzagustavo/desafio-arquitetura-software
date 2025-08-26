using CashFlow.Application.Transactions.Handlers;
using CashFlow.Domain.Transactions;
using Riok.Mapperly.Abstractions;

namespace CashFlow.Application.Transactions
{
    [Mapper]
    public partial class TransactionMapper
    {
        public partial TransactionEntity ToTransactionEntity(CreateTransactionRequest transactionRequest);

        public partial GetTransactionResponse ToTransactionResponse(TransactionEntity transactionEntity);
    }
}
