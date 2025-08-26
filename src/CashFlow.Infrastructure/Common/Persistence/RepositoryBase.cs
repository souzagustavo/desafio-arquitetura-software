using CashFlow.Application.Common;
using CashFlow.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CashFlow.Infrastructure.Common.Persistence
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly CashFlowDbContext _context;
        
        protected DbSet<T> DbSet => _context.Set<T>();

        protected RepositoryBase(CashFlowDbContext context) => _context = context;

        public virtual async Task<PagedResult<TResult>> GetPagedAsync<TResult>(
            PaginationFilter filter,
            Expression<Func<T, TResult>>? selector = null,
            Func<IQueryable<T>, IQueryable<T>>? queryBuilder = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = DbSet.AsQueryable();

            if (queryBuilder is not null)
                query = queryBuilder(query);

            var totalItems = await query.CountAsync(cancellationToken);

            List<TResult> items;

            if (selector is not null)
            {
                items = await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(selector)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                items = await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Cast<TResult>()
                    .ToListAsync(cancellationToken);
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

            return new PagedResult<TResult>
            {
                Items = items,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

    }
}

