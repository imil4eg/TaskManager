using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManager.Application.Commands;
using TaskManager.Commands;
using TaskManager.Domain;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public sealed class ProjectController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITableQueries<ProjectEntity, ProjectViewModel> _projectQueries;
        private readonly ITableQueries<TaskEntity, TaskViewModel> _taskQueries;

        public ProjectController(IMediator mediator, 
            ITableQueries<ProjectEntity, ProjectViewModel> projectQueries,
            ITableQueries<TaskEntity, TaskViewModel> taskQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _projectQueries = projectQueries ?? throw new ArgumentNullException(nameof(projectQueries));
            _taskQueries = taskQueries ?? throw new ArgumentNullException(nameof(taskQueries));
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(CreateProjectCommand createCommand)
        {
            if (string.IsNullOrEmpty(createCommand.Name))
            {
                return BadRequest("Project name must not be empty.");
            }

            try
            {
                var result = await _mediator.Send<ProjectViewModel>(createCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var projects = await _projectQueries.GetAllAsync();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var project = await _projectQueries.GetByRowKeyAsync(id);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Project name must not be empty.");
            }

            try
            {
                var project = await _projectQueries.GetByPartitionKeyAsync(name);
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{projectId:guid}/tasks")]
        public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("ProjectId can't be empty.");
            }

            try
            {
                var result = await _taskQueries.GetByPartitionKeyAsync(projectId.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProjectCommand updateCommand)
        {
            if (updateCommand.Id == Guid.Empty)
            {
                return BadRequest("Id can't be empty.");
            }

            try
            {
                var result = await _mediator.Send<ProjectViewModel>(updateCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteAsync(DeleteProjectCommand deleteCommand)
        {
            if (deleteCommand.Id == Guid.Empty)
            {
                return BadRequest("Id can't be empty.");
            }

            if (string.IsNullOrEmpty(deleteCommand.Name))
            {
                return BadRequest("Name can't be empty.");
            }

            try
            {
                await _mediator.Send(deleteCommand);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
