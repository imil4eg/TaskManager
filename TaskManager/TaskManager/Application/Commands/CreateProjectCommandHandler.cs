using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Domain;

namespace TaskManager.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectViewModel>
    {
        private readonly IGenericRepository<ProjectEntity> _repository;

        public CreateProjectCommandHandler(IGenericRepository<ProjectEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ProjectViewModel> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entitiy = new ProjectEntity
            {
                RowKey = request.Id.ToString(),
                PartitionKey = request.Name,
                Code = request.Code
            };

            var createdEntitiy = await _repository.CreateAsync(entitiy, cancellationToken);

            var viewModel = new ProjectViewModel
            {
                Id = Guid.Parse(createdEntitiy.RowKey),
                Name = createdEntitiy.PartitionKey,
                Code = createdEntitiy.Code
            };

            return viewModel;
        }
    }
}
