using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.DAL
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> GetAsync(string rowKey, string partitionKey, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> UpdateOrInsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
    }
}
