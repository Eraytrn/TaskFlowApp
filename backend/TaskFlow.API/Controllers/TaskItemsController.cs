using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.TaskItems.Commands.CreateTaskItem;
using TaskFlow.Application.Features.TaskItems.Commands.UpdateTaskItem;
using TaskFlow.Application.Features.TaskItems.Commands.DeleteTaskItem;
using TaskFlow.Application.Features.TaskItems.Commands.ChangeTaskStatus;
using TaskFlow.Application.Features.TaskItems.Commands.AssignTaskToUser;
using TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemById;
using TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemsByBoardId;
using TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemsByUserId;
using TaskFlow.Application.Features.TaskItems.Queries.GetAllTaskItems;
using TaskFlow.Application.Features.TaskItems.Commands.AssignLabelToTask;
using TaskFlow.Application.Features.TaskItems.Commands.RemoveLabelFromTask;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tüm task'ları sayfalı olarak getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllTaskItemsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// ID'ye göre task getir (detaylı, comment'larla birlikte)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetTaskItemByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Board'a ait task'ları getir
    /// </summary>
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetByBoardId(int boardId)
    {
        var query = new GetTaskItemsByBoardIdQuery { BoardId = boardId };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Kullanıcıya atanmış task'ları getir
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetTaskItemsByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Yeni task oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    /// <summary>
    /// Task bilgilerini güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskItemCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// Task'ın status'unu değiştir
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeTaskStatusCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// Task'ı kullanıcıya ata
    /// </summary>
    [HttpPatch("{id}/assign")]
    public async Task<IActionResult> AssignToUser(int id, [FromBody] AssignTaskToUserCommand command)
    {
        if (id != command.TaskItemId)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// Task'ı sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteTaskItemCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// Task'a etiket ata
    /// </summary>
    [HttpPost("{id}/labels")]
    public async Task<IActionResult> AssignLabel(int id, [FromBody] AssignLabelToTaskCommand command)
    {
        if (id != command.TaskId)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return Ok(result);
    }

    /// <summary>
    /// Task'tan etiketi kaldır
    /// </summary>
    [HttpDelete("{id}/labels/{labelId}")]
    public async Task<IActionResult> RemoveLabel(int id, int labelId)
    {
        var command = new RemoveLabelFromTaskCommand { TaskId = id, LabelId = labelId };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return NoContent();
    }
}
