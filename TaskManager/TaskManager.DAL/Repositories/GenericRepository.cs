using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.DAL
{
    public sealed class GenericRepository<TEntity> : IGenericRepository<TEntity>
           where TEntity : TableEntity
    {
        private readonly CloudTable _cloudTable;

        public GenericRepository(ICloudTableFactory cloudTableFactory)
        {
            if (cloudTableFactory == null)
            {
                throw new ArgumentNullException(nameof(cloudTableFactory));
            }

            _cloudTable = cloudTableFactory.GetTable(typeof(TEntity));
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            cancellationToken.ThrowIfCancellationRequested();

            TableOperation insertOperation = TableOperation.Insert(entity);
            TableResult result = await _cloudTable.ExecuteAsync(insertOperation);
            TEntity insertedProject = result.Result as TEntity;

            return insertedProject;
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            TableOperation retrieveOperation = TableOperation.Retrieve<TEntity>(null, id.ToString());
            TableResult result = await _cloudTable.ExecuteAsync(retrieveOperation);
            TEntity entitiy = result.Result as TEntity;

            return entitiy;
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            cancellationToken.ThrowIfCancellationRequested();

            TableOperation deleteOperation = TableOperation.Delete(entity);
            TableResult result = await _cloudTable.ExecuteAsync(deleteOperation, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            cancellationToken.ThrowIfCancellationRequested();

            TableOperation updateOperation = TableOperation.Merge(entity);
            TableResult result = await _cloudTable.ExecuteAsync(updateOperation, cancellationToken);

            return result.Result as TEntity;
        }
    }
}
