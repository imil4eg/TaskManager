using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager
{
    public interface ITableQueries<TEntity, TViewModel> where TEntity : TableEntity, new()
    {
        Task<TViewModel> GetAsync(string rowKey, string partitionKey, CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> GetEntityAsync(string rowKey, string partitionKey, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<TViewModel>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<TViewModel>> GetByRowKeyAsync(Guid rowKey, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<TViewModel>> GetByPartitionKeyAsync(string partitionKey, CancellationToken cancellationToken = default(CancellationToken));
    }
}
