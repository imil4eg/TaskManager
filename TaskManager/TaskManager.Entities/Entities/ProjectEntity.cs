using Microsoft.Azure.Cosmos.Table;

namespace TaskManager.Domain
{
    public sealed class ProjectEntity : TableEntity
    {
        public ProjectEntity()
        {
        }

        public ProjectEntity(int id, string name)
        {
            RowKey = id.ToString();
            PartitionKey = name;
        }

        public string Code { get; set; }
    }
}
