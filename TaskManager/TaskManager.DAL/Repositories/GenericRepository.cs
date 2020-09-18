using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Domain;

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

            _cloudTable = cloudTableFactory.GetTable<TEntity>();
            _cloudTable.CreateIfNotExists();
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

        public async Task<TEntity> UpdateOrInsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (entity == null) 
            {
                throw new ArgumentNullException(nameof(entity));
            }

            cancellationToken.ThrowIfCancellationRequested();

            TableOperation updateOrInsertOperation = TableOperation.InsertOrMerge(entity);
            TableResult result = await _cloudTable.ExecuteAsync(updateOrInsertOperation, cancellationToken);

            return result.Result as TEntity;
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
