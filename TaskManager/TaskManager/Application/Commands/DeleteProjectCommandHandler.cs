using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Domain;

namespace TaskManager.Application.Commands
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IGenericRepository<ProjectEntity> _repository;
        private readonly ITableQueries<ProjectEntity, ProjectViewModel> _queries;

        public DeleteProjectCommandHandler(IGenericRepository<ProjectEntity> repository,
            ITableQueries<ProjectEntity, ProjectViewModel> queries)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await _queries.GetEntityAsync(request.Id.ToString(), request.Name, cancellationToken);

            await _repository.DeleteAsync(entity, cancellationToken);

            return Unit.Value;
        }
    }
}
