using CashFlow.Domain.Transactions;

namespace CashFlow.Infrastructure.Transaction
{
    public interface ITransationsRepository
    {
        Task AddAsync(TransactionEntity entity, CancellationToken cancellationToken);

        Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}