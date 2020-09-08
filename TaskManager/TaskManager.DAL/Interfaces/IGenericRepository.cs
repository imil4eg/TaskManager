using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.DAL
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
