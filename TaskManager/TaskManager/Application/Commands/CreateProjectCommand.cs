using MediatR;
using System;

namespace TaskManager.Commands
{
    public class CreateProjectCommand : IRequest<ProjectViewModel>
    {
        public Guid Id { get; }

        public string Name { get; set; }

        public string Code { get; set; }

        public CreateProjectCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}
