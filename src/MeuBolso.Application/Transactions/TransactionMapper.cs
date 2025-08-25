using MeuBolso.Application.Transactions.Handlers;
using MeuBolso.Domain.Transactions;
using Riok.Mapperly.Abstractions;

namespace MeuBolso.Application.Transactions
{
    [Mapper]
    public partial class TransactionMapper
    {
        public partial TransactionEntity ToTransactionEntity(CreateTransactionRequest transactionRequest);

        public partial GetTransactionResponse ToGetTransactionResponse(TransactionEntity transactionEntity);
    }
}
