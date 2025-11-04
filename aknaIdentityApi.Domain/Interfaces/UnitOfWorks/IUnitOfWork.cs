

namespace aknaIdentityApi.Domain.Interfaces.UnitOfWorks
{
    /// <summary>
    /// Interface for Unit of Work pattern
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        Task<int> CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
