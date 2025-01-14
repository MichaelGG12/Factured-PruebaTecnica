using Factured_PruebaTecnica_API.Models;
using Factured_PruebaTecnica_API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Factured_PruebaTecnica_API.Controllers
{
    [Route("api/task")] [ApiController] 
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DataSyncService _dataSyncService;

        public TasksController(IMediator mediator, DataSyncService dataSyncService)
        {
            _mediator = mediator;
            _dataSyncService = dataSyncService;
        }

        #region Create

        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
        {
            if (createTaskCommand == null)
            {
                return BadRequest("Invalid data.");
            }         
            var result = await _mediator.Send(createTaskCommand); // Send the command to the handler using mediator      
            await _dataSyncService.SyncDataAsync(cancellationToken); // Trigger synchronization
            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
        }

        #endregion

        #region Read

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTasks([FromQuery] int boardId = 0)
        {
            var result = await _mediator.Send(new GetTaskQuery { BoardId = boardId });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetTaskByIdQuery { Id = id });
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Return NotFound if board doesn't exist
            }
        }

        #endregion

        #region Update

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand updateTaskModel, CancellationToken cancellationToken)
        {
            if (updateTaskModel == null)
            {
                return BadRequest("Invalid data.");
            }
            var result = await _mediator.Send(updateTaskModel);
            await _dataSyncService.SyncDataAsync(cancellationToken); // Trigger synchronization
            return Ok(result);
        }

        #endregion

        #region Delete

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _mediator.Send(new DeleteTaskCommand { Id = id });
                if (success)
                {
                    await _dataSyncService.SyncDataAsync(cancellationToken);
                    return Ok(new { Message = "Task successfully deleted." });
                }
                return BadRequest("Failed to delete the task.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        #endregion
    }
}