namespace CashFlow.Application.Transactions
{
    public interface ITransactionLockProcessor
    {
        Task DoAsync(Guid transactionId, CancellationToken cancellationToken);
    }
}
