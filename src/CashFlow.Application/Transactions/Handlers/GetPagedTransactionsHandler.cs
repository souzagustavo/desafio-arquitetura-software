using CashFlow.Application.Common;
using CashFlow.Application.Common.Handlers;
using CashFlow.Application.Common.Interfaces;
using CashFlow.Domain.Transactions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Application.Transactions.Handlers;

public class GetPagedTransactionsQuery : PaginationFilter
{
    public ETransactionType? Type { get; init; }
    public EPaymentMethod? PaymentMethod { get; init; }
}

public interface IGetPagedTransactionHandler : IHandler
{
    Task<ErrorOr<PagedResult<GetTransactionResponse>>> HandleAsync(
         Guid userId,
         Guid accountId,
         GetPagedTransactionsQuery query,
         CancellationToken cancellationToken);
}

public class GetPagedTransactionsHandler : IGetPagedTransactionHandler
{
    private readonly ICashFlowDbContext _cashFlowDbContext;

    public GetPagedTransactionsHandler(ICashFlowDbContext cashFlowDbContext)
    {
        _cashFlowDbContext = cashFlowDbContext;
    }

    public async Task<ErrorOr<PagedResult<GetTransactionResponse>>> HandleAsync(
        Guid userId,
        Guid accountId,
        GetPagedTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var queryble = _cashFlowDbContext.Accounts
            .Where(s => s.Id == accountId && s.IdentityUserId == userId)
            .Include(s => s.Transactions)
            .SelectMany(s => s.Transactions);

        if (request.Type.HasValue)
            queryble = queryble.Where(t => t.Type == request.Type.Value);

        if (request.PaymentMethod.HasValue)
            queryble = queryble.Where(t => t.PaymentMethod == request.PaymentMethod.Value);

        var total = queryble.Count();

        queryble = queryble
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);            

        var results = await queryble.ToListAsync(cancellationToken);

        var mapper = new TransactionMapper();
        var items = results
            .Select(t => mapper.ToTransactionResponse(t))
            .ToList();

        return new PagedResult<GetTransactionResponse>
        {
            Items = items,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalItems = total
        };
    }
}
