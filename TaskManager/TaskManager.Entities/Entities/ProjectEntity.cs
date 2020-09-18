using Microsoft.Azure.Cosmos.Table;
using System;

namespace TaskManager.Domain
{
    public sealed class ProjectEntity : TableEntity
    {
        public ProjectEntity()
        {
        }

        public ProjectEntity(Guid id, string name)
        {
            RowKey = id.ToString();
            PartitionKey = name;
        }

        public string Code { get; set; }
    }
}
