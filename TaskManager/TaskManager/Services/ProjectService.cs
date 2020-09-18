using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain;
using TaskManager.Exceptions;
using TaskManager.Interfaces;

namespace TaskManager.Services
{
    public sealed class ProjectService : IProjectService
    {
        private readonly ITableQueries<ProjectEntity, ProjectViewModel> _projectQueries;

        public ProjectService(ITableQueries<ProjectEntity, ProjectViewModel> projectQueries)
        {
            _projectQueries = projectQueries ?? throw new ArgumentNullException(nameof(projectQueries));
        }

        public async Task ThrowIfProjectDoesNotExists(Guid projectId)
        {
            var entity = await _projectQueries.GetByRowKeyAsync(projectId);

            if (entity == null || 
                !entity.Any())
            {
                throw new DataNotFoundException(string.Format("Project with ID {0} not found.", projectId));
            }
        }
    }
}
