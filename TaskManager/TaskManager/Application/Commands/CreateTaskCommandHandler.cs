using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Domain;
using TaskManager.Interfaces;

namespace TaskManager.Application.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskViewModel>
    {
        private readonly IProjectService _projectService;
        private readonly IGenericRepository<TaskEntity> _repository;

        public CreateTaskCommandHandler(IGenericRepository<TaskEntity> repository,
            IProjectService projectService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        public async Task<TaskViewModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _projectService.ThrowIfProjectDoesNotExists(request.ProjectId);

            var entity = new TaskEntity
            {
                RowKey = request.Id.ToString(),
                PartitionKey = request.ProjectId.ToString(),
                Name = request.Name,
                Description = request.Description
            };

            var createdEntity = await _repository.CreateAsync(entity, cancellationToken);

            var viewModel = new TaskViewModel
            {
                Id = Guid.Parse(createdEntity.RowKey),
                ProjectId = Guid.Parse(createdEntity.PartitionKey),
                Name = createdEntity.Name,
                Description = createdEntity.Description
            };

            return viewModel;
        }
    }
}
