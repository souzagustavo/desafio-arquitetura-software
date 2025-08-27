using CashFlow.Application.Transactions;
using CashFlow.Domain.Transactions;
using MassTransit;

namespace CashFlow.Consumer.Worker.Consumers
{
    public class ProcessTransactionCreated : IConsumer<TransactionCreated>
    {
        private readonly ILogger<ProcessTransactionCreated> _logger;
        private readonly ITransactionLockProcessor _transactionLockProcessor;
        public ProcessTransactionCreated(ILogger<ProcessTransactionCreated> logger,
            ITransactionLockProcessor transactionLockProcessor)
        {
            _logger = logger;
            _transactionLockProcessor = transactionLockProcessor;
        }

        public async Task Consume(ConsumeContext<TransactionCreated> context)
        {
            _logger.LogInformation("Processing transaction created event for transaction id: {TransactionId}", context.Message.Id);

            var transactionId = context.Message.Id;

            await _transactionLockProcessor.DoAsync(transactionId, context.CancellationToken);

            _logger.LogInformation("Finished processing transaction created event for transaction id: {TransactionId}", context.Message.Id);
        }
    }
}
