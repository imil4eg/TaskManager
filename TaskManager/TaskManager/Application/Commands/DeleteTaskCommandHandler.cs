using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Domain;
using TaskManager.Exceptions;

namespace TaskManager.Application.Commands
{
    public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        private readonly IGenericRepository<TaskEntity> _repository;
        private readonly ITableQueries<TaskEntity, TaskViewModel> _queries;

        public DeleteTaskCommandHandler(IGenericRepository<TaskEntity> repository,
            ITableQueries<TaskEntity, TaskViewModel> queries)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var existingTask = await _queries.GetEntityAsync(request.Id.ToString(), request.ProjectId.ToString(), cancellationToken);

            if (existingTask == null)
            {
                throw new DataNotFoundException(string.Format("Task with ID {0} not found.", request.Id));
            }

            await _repository.DeleteAsync(existingTask, cancellationToken);

            return Unit.Value;
        }
    }
}
