using AutoMapper;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table.Queryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.DAL;

namespace TaskManager.Application.Queries
{
    public class TableQueries<TEntity, TViewModel> : ITableQueries<TEntity, TViewModel>
           where TEntity : TableEntity, new()
    {
        private readonly CloudTable _table;
        private readonly IMapper _mapper;

        public TableQueries(ICloudTableFactory cloudTableFactory, IMapper mapper)
        {
            if (cloudTableFactory == null)
            {
                throw new ArgumentNullException(nameof(cloudTableFactory));
            }

            _table = cloudTableFactory.GetTable<TEntity>();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _table.CreateQuery<TEntity>();
            var entities = await query.ExecuteQueryAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(entities);
        }

        public async Task<TViewModel> GetAsync(string rowKey, string partitionKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entity = await GetEntityAsync(rowKey, partitionKey, cancellationToken);

            return _mapper.Map<TEntity, TViewModel>(entity);
        }

        public async Task<TEntity> GetEntityAsync(string rowKey, string partitionKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(rowKey))
            {
                throw new ArgumentNullException(nameof(rowKey));
            }

            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            cancellationToken.ThrowIfCancellationRequested();

            TableOperation retrieveOperation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            TableResult result = await _table.ExecuteAsync(retrieveOperation, cancellationToken);

            var entity = result.Result as TEntity;

            return entity;
        }

        public async Task<IEnumerable<TViewModel>> GetByRowKeyAsync(Guid rowKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _table.CreateQuery<TEntity>()
                             .Where(e => string.Equals(e.RowKey, rowKey.ToString()))
                             .AsTableQuery();
            var result = await query.ExecuteQueryAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(result);
        }

        public async Task<IEnumerable<TViewModel>> GetByPartitionKeyAsync(string partitionKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _table.CreateQuery<TEntity>()
                             .Where(e => string.Equals(e.PartitionKey, partitionKey))
                             .AsTableQuery();
            var result = await query.ExecuteQueryAsync(cancellationToken);

            return _mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(result);
        }
    }
}
