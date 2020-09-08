using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace TaskManager.Domain
{
    public sealed class ProjectEntity : TableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public IReadOnlyCollection<TaskEntity> Tasks { get; set; } 
    }
}
