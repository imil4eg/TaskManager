using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.Cosmos.Table.Queryable;

namespace TaskManager
{
    public static class TableQueryExtensions
    {
        public static async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(this TableQuery<TEntity> tableQuery, CancellationToken cancellationToken)
        {
            var query = tableQuery;
            var continuationToken = default(TableContinuationToken);
            var result = new List<TEntity>();

            do
            {
                var queryResult = await query.ExecuteSegmentedAsync(continuationToken, cancellationToken);

                result.Capacity += queryResult.Results.Count;

                result.AddRange(queryResult.Results);

                if (continuationToken == null || !tableQuery.TakeCount.HasValue)
                {
                    continue;
                }

                var itemsToLoad = tableQuery.TakeCount.Value - result.Count;

                query = itemsToLoad > 0 ?
                        tableQuery.Take<TEntity>(itemsToLoad).AsTableQuery() :
                        null;
            } while (continuationToken != null && query != null && !cancellationToken.IsCancellationRequested);

            return result;
        }
    }
}
