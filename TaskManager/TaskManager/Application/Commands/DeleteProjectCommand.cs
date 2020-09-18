using MediatR;
using System;

namespace TaskManager.Application.Commands
{
    public sealed class DeleteProjectCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
