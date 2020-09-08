using Microsoft.Azure.Cosmos.Table;

namespace TaskManager.Domain
{
    public sealed class TaskEntity : TableEntity
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
