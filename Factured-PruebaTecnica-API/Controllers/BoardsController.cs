using Factured_PruebaTecnica_API.DTO;
using Factured_PruebaTecnica_API.Models;
using Factured_PruebaTecnica_API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Factured_PruebaTecnica_API.Controllers
{
    [Route("api/board")] [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DataSyncService _dataSyncService;

        public BoardsController(IMediator mediator, DataSyncService dataSyncService)
        {
            _mediator = mediator;
            _dataSyncService = dataSyncService;
        }

        #region Create

        [HttpPost("create")]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardCommand createBoardModel, CancellationToken cancellationToken)
        {
            if (createBoardModel == null)
            {
                return BadRequest("Invalid data.");
            }
            var result = await _mediator.Send(createBoardModel); // Send the command to the handler using mediator     
            await _dataSyncService.SyncDataAsync(cancellationToken); // Trigger synchronization
            return CreatedAtAction(nameof(GetBoardById), new { id = result.Id }, result);
        }

        #endregion

        #region Read

        [HttpGet("all")]
        public async Task<IActionResult> GetBoards()
        {
            var boards = await _mediator.Send(new GetBoardsQuery()); // Pass a query object here
            return Ok(boards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardById(int id)
        {
            try
            {
                var result = await _mediator.Send(new GetBoardByIdQuery(id)); // Send the command to the handler using mediator  
                return Ok(result); // Return the board as a response
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Return NotFound if board doesn't exist
            }
        }

        #endregion

        #region Update

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBoard(int id, [FromBody] UpdateBoardCommand updateBoardModel, CancellationToken cancellationToken)
        {
            if (updateBoardModel == null)
            {
                return BadRequest("Invalid data.");
            }
            var result = await _mediator.Send(updateBoardModel);         
            await _dataSyncService.SyncDataAsync(cancellationToken); // Trigger synchronization
            return Ok(result);
        }

        #endregion

        #region Delete

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBoard(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mediator.Send(new DeleteBoardCommand { Id = id });
                if (result)
                {
                    await _dataSyncService.SyncDataAsync(cancellationToken);
                    return Ok(new { Message = "Board successfully deleted." });
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