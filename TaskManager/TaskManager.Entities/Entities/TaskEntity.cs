using Microsoft.Azure.Cosmos.Table;
using System;

namespace TaskManager.Domain
{
    public sealed class TaskEntity : TableEntity
    {
        public TaskEntity()
        {
        }

        public TaskEntity(Guid id, Guid projectId)
        {
            RowKey = id.ToString();
            PartitionKey = projectId.ToString();
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
