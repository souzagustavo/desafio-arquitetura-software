namespace CashFlow.Application.Transactions
{
    public interface ITransactionLockProcessor
    {
        Task DoAsync(Guid userId, Guid transactionId, CancellationToken cancellationToken);
    }
}
