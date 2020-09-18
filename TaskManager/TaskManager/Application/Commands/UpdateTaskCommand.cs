using MediatR;
using System;

namespace TaskManager.Application.Commands
{
    public class UpdateTaskCommand : IRequest<TaskViewModel>
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
