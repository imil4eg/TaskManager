using Microsoft.Azure.Cosmos.Table;

namespace TaskManager.Domain
{
    public sealed class TaskEntity : TableEntity
    {
        public TaskEntity()
        {
        }

        public TaskEntity(int id, int projectId)
        {
            RowKey = id.ToString();
            PartitionKey = projectId.ToString();
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
