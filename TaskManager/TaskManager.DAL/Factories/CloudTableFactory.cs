using Microsoft.Azure.Cosmos.Table;
using System;
using TaskManager.Domain;

namespace TaskManager.DAL
{
    public sealed class CloudTableFactory : ICloudTableFactory
    {
        private readonly CloudTableClient _cloudTableClient;

        public CloudTableFactory(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
        }

        public CloudTable GetTable(string tableName)
        {
            CloudTable cloudTable;
            switch (tableName)
            {
                case TableNames.ProjectTable:
                case TableNames.TaskTable:
                    cloudTable = _cloudTableClient.GetTableReference(tableName);
                    break;
                default:
                    throw new NotImplementedException("Table does not exist.");
            }

            return cloudTable;
        }

        public CloudTable GetTable<TEntity>() where TEntity : TableEntity
        {
            switch(typeof(TEntity).Name)
            {
                case nameof(ProjectEntity):
                    return GetTable(TableNames.ProjectTable);
                case nameof(TaskEntity):
                    return GetTable(TableNames.TaskTable);
                default:
                    throw new NotImplementedException(nameof(TEntity));
            }
        }
    }
}
