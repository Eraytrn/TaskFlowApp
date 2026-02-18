using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Comments.Commands.CreateComment;
using TaskFlow.Application.Features.Comments.Commands.DeleteComment;
using TaskFlow.Application.Features.Comments.Queries.GetCommentsByTaskId;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Bir task'a ait tüm yorumları getir
    /// </summary>
    [HttpGet("task/{taskItemId}")]
    public async Task<IActionResult> GetByTaskItemId(int taskItemId)
    {
        var query = new GetCommentsByTaskIdQuery { TaskItemId = taskItemId };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Yeni yorum oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetByTaskItemId), new { taskItemId = result.Data.TaskItemId }, result.Data);
    }

    /// <summary>
    /// Yorumu sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteCommentCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}
