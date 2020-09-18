using MediatR;
using System;

namespace TaskManager.Application.Commands
{
    public class CreateTaskCommand : IRequest<TaskViewModel>
    {
        public Guid Id { get; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CreateTaskCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}
