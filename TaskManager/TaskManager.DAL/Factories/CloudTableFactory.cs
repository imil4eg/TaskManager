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

        public CloudTable GetTable(Type tableType)
        {
            if (tableType is TaskEntity)
            {
                return GetTable(TableNames.TaskTable);
            }
            else if (tableType is ProjectEntity)
            {
                return GetTable(TableNames.ProjectTable);
            }
            else
            {
                throw new NotImplementedException(nameof(tableType));
            }
        }
    }
}
