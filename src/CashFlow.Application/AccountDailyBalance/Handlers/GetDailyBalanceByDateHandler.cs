using CashFlow.Application.Account;
using CashFlow.Application.Common.Handlers;
using ErrorOr;

namespace CashFlow.Application.AccountDailyBalance.Handlers;

public record GetAccountDailyBalanceQuery(Guid UserId, Guid AccountId, DateOnly Date);

public record GetAccountDailyBalanceResponse(Guid Id, DateOnly Date,
    decimal TotalIncoming, decimal TotalOutgoing, decimal BalanceOfDay, DateTimeOffset? UpdatedAt);

public interface IGetDailyBalanceByDateHandler : IHandler
{
    Task<ErrorOr<GetAccountDailyBalanceResponse>> HandleAsync(
        Guid userId,
        DateOnly date,
        CancellationToken cancellationToken);
}

public class GetDailyBalanceByDateHandler : IGetDailyBalanceByDateHandler
{
    private readonly IAccountCachedRepository _accountCachedRepository;

    public GetDailyBalanceByDateHandler(IAccountCachedRepository accountCachedRepository)
    {
        _accountCachedRepository = accountCachedRepository;
    }

    public async Task<ErrorOr<GetAccountDailyBalanceResponse>> HandleAsync(
        Guid userId,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        //var result = await
        //    _dailyBalanceCachedQueries.GetByDateAsync(userId, date, cancellationToken);

        //var mapper = new DailyBalanceMapper();
        //return mapper.ToResponse(result);

        throw new NotImplementedException();
    }
}
