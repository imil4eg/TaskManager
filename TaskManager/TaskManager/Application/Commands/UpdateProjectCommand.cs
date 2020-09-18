using MediatR;
using System;

namespace TaskManager
{
    public sealed class UpdateProjectCommand : IRequest<ProjectViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public UpdateProjectCommand()
        {
        }


    }
}
