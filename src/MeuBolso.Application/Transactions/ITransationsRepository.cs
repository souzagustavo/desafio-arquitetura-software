using MeuBolso.Domain.Transactions;

namespace MeuBolso.Application.Transactions
{
    public interface ITransationsRepository
    {
        Task AddAsync(TransactionEntity entity, CancellationToken cancellationToken);

        Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
