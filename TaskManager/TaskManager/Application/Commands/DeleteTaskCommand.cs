using MediatR;
using System;

namespace TaskManager.Application.Commands
{
    public sealed class DeleteTaskCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
    }
}
