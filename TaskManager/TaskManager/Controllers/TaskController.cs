using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManager.Application.Commands;
using TaskManager.Domain;
using TaskManager.Exceptions;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITableQueries<TaskEntity, TaskViewModel> _tasksTableQueries;

        public TaskController(IMediator mediator,
            ITableQueries<TaskEntity, TaskViewModel> taskTableQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _tasksTableQueries = taskTableQueries ?? throw new ArgumentNullException(nameof(taskTableQueries));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var tasks = await _tasksTableQueries.GetAllAsync();

                return Ok(tasks);
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
            if (id == Guid.Empty)
            {
                return BadRequest("Id can't be empty.");
            }

            try
            {
                var task = await _tasksTableQueries.GetByRowKeyAsync(id);
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskCommand createCommand)
        {
            if (string.IsNullOrEmpty(createCommand.Name))
            {
                return BadRequest("Name can't be empty.");
            }

            try
            {
                var result = await _mediator.Send(createCommand);
                return Ok(result);
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateTaskCommand updateCommand)
        {
            if (updateCommand.Id == Guid.Empty)
            {
                return BadRequest("Id can't be empty.");
            }

            if (updateCommand.ProjectId == Guid.Empty)
            {
                return BadRequest("Project Id can't be empty.");
            }

            if (string.IsNullOrEmpty(updateCommand.Name))
            {
                return BadRequest("Name can't be empty.");
            }

            try
            {
                var result = await _mediator.Send(updateCommand);
                return Ok(result);
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteTaskCommand deleteCommand)
        {
            if (deleteCommand.Id == Guid.Empty)
            {
                return BadRequest("Id can't be empty.");
            }

            if (deleteCommand.ProjectId == Guid.Empty)
            {
                return BadRequest("Project Id can't be empty.");
            }

            try
            {
                await _mediator.Send(deleteCommand);
                return Ok();
            }
            catch (DataNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
