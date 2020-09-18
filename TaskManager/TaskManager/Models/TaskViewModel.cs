using System;

namespace TaskManager
{
    public sealed class TaskViewModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
