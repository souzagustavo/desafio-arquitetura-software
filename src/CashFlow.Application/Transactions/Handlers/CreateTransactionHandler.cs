using ErrorOr;
using CashFlow.Application.Common.Handlers;
using CashFlow.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using CashFlow.Application.Common.Interfaces;

namespace CashFlow.Application.Transactions.Handlers;

public record CreateTransactionRequest(ETransactionType Type,
    EPaymentMethod PaymentMethod, decimal TotalAmount, string? Notes);

public record CreateTransactionResponse(Guid Id);

public interface ICreateTransationHandler : IHandler
{
    Task<ErrorOr<CreateTransactionResponse>> HandleAsync(
        Guid UserId,
        Guid AccountId,
        CreateTransactionRequest request,
        CancellationToken cancellationToken);
}

public class CreateTransactionHandler : ICreateTransationHandler
{
    private readonly ICashFlowDbContext _dbContext;

    public CreateTransactionHandler(ICashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<CreateTransactionResponse>> HandleAsync(
        Guid UserId, Guid AccountId,
        CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var isValidAccount =
            await _dbContext.Accounts.AnyAsync(s => s.Id == AccountId && s.IdentityUserId == UserId, cancellationToken);

        if (!isValidAccount)
            return Error.NotFound(description: "Account not found.");

        var mapper = new TransactionMapper();
        var entity = mapper.ToTransactionEntity(request);

        entity.AccountId = AccountId;

        await _dbContext.Transactions.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTransactionResponse(entity.Id);
    }
}
