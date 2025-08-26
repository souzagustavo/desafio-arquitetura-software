using System.Linq.Expressions;

namespace CashFlow.Application.Common.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<PagedResult<TResult>> GetPagedAsync<TResult>(
            PaginationFilter filter,
            Expression<Func<T, TResult>>? selector = null,
            Func<IQueryable<T>, IQueryable<T>>? queryBuilder = null,
            CancellationToken cancellationToken = default);
    }
}
