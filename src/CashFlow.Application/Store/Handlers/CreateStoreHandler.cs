using CashFlow.Application.Common.Handlers;
using CashFlow.Application.Common.Interfaces;
using ErrorOr;

namespace CashFlow.Application.Store.Handlers
{
    public record CreateStoreRequest(string Name);

    public record CreatedStoreResponse(Guid Id);

    public interface ICreateStoreHandler : IHandler
    {
        Task<ErrorOr<CreatedStoreResponse>> HandleAsync(
            Guid userId,
            CreateStoreRequest request,
            CancellationToken cancellationToken);
    }

    public class CreateStoreHandler : ICreateStoreHandler
    {
        private readonly ICashFlowDbContext _dbContext;

        public CreateStoreHandler(ICashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<CreatedStoreResponse>> HandleAsync(Guid userId, CreateStoreRequest request, CancellationToken cancellationToken)
        {
            var mapper = new StoreMapper();
            var entity = mapper.ToStoreEntity(request);

            entity.IdentityUserId = userId;

            await _dbContext.Stores.AddAsync(entity, cancellationToken);
            await _dbContext.StoreBalances.AddAsync(new() { Store = entity }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedStoreResponse(entity.Id);
        }
    }
}
