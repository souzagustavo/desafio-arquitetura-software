using CashFlow.Application.Common.Interfaces;
using CashFlow.Domain.Transactions;

namespace CashFlow.Application.Transactions
{
    public interface ITransationsRepository : IRepositoryBase<TransactionEntity>
    {
        Task AddAsync(TransactionEntity entity, CancellationToken cancellationToken);

        Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}