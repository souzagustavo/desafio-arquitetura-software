using ErrorOr;
using CashFlow.Application.Common.Handlers;

namespace CashFlow.Application.Transactions.Handlers;

public record GetTransactionResponse(Guid Id, DateTimeOffset OccurrentAt,
    ETransactionType Type, decimal Amount, string? Description);

public interface IGetTransactionByIdHandler : IHandler
{
    Task<ErrorOr<GetTransactionResponse>> HandleAsync(
        Guid id,
        CancellationToken cancellationToken);
}

public class GetTransactionByIdHandler : IGetTransactionByIdHandler
{
    private readonly ITransationsRepository _transationsRepository;

    public GetTransactionByIdHandler(ITransationsRepository transationsRepository)
    {
        _transationsRepository = transationsRepository;
    }

    public async Task<ErrorOr<GetTransactionResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _transationsRepository.GetByIdAsync(id, cancellationToken);

        if (result is null)
            return Error.NotFound(description: "Transaction not found.");

        var mapper = new TransactionMapper();
        var response = mapper.ToGetTransactionResponse(result);

        return response;
    }
}
