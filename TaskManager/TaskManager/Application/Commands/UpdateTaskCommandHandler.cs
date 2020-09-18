using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Domain;
using TaskManager.Interfaces;

namespace TaskManager.Application.Commands
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskViewModel>
    {
        private readonly IGenericRepository<TaskEntity> _repository;
        private readonly ITableQueries<TaskEntity, TaskViewModel> _taskQueries;
        private readonly IProjectService _projectService;

        public UpdateTaskCommandHandler(IGenericRepository<TaskEntity> repository,
            ITableQueries<TaskEntity, TaskViewModel> taskQueries,
            IProjectService projectService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _taskQueries = taskQueries ?? throw new ArgumentNullException(nameof(taskQueries));
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        public async Task<TaskViewModel> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _projectService.ThrowIfProjectDoesNotExists(request.ProjectId);

            var existingTask = await _taskQueries.GetEntityAsync(request.Id.ToString(), request.ProjectId.ToString(), cancellationToken);

            existingTask.Name = request.Name;
            existingTask.Description = existingTask.Description;

            var updatedTask = await _repository.UpdateOrInsertAsync(existingTask, cancellationToken);

            var viewModel = new TaskViewModel
            {
                Id = Guid.Parse(updatedTask.RowKey),
                ProjectId = Guid.Parse(updatedTask.PartitionKey),
                Name = updatedTask.Name,
                Description = updatedTask.Description
            };

            return viewModel;
        }
    }
}
