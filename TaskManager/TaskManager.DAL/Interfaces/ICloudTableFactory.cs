using Microsoft.Azure.Cosmos.Table;
using System;

namespace TaskManager.DAL
{
    public interface ICloudTableFactory
    {
        CloudTable GetTable(string tableName);

        CloudTable GetTable<TEntity>() where TEntity : TableEntity;
    }
}
