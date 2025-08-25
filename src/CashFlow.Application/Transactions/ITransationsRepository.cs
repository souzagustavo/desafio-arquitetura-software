using CashFlow.Domain.Transactions;

namespace CashFlow.Application.Transactions
{
    public interface ITransationsRepository
    {
        Task AddAsync(TransactionEntity entity, CancellationToken cancellationToken);

        Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
