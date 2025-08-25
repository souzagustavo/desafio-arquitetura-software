using ErrorOr;
using CashFlow.Application.Common.Handlers;

namespace CashFlow.Application.Transactions.Handlers;

public record CreateTransactionRequest(Guid StoreId, ETransactionType Type, decimal Amount, string? Description)
{
    public DateTimeOffset OccurrentAt { get; private set; } = DateTimeOffset.UtcNow;
}

public record CreateTransactionResponse(Guid Id);

public interface ICreateTransationHandler : IHandler
{
    Task<ErrorOr<CreateTransactionResponse>> HandleAsync(
    CreateTransactionRequest request,
    CancellationToken cancellationToken);
}

public class CreateTransactionHandler : ICreateTransationHandler
{
    private readonly ITransationsRepository _transationsRepository;

    public CreateTransactionHandler(ITransationsRepository transationsRepository)
    {
        _transationsRepository = transationsRepository;
    }

    public async Task<ErrorOr<CreateTransactionResponse>> HandleAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var mapper = new TransactionMapper();
        var entity = mapper.ToTransactionEntity(request);

        await _transationsRepository.AddAsync(entity, cancellationToken);

        return new CreateTransactionResponse(entity.Id);
    }
}
