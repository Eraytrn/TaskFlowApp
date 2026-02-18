using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Boards.Commands.CreateBoard;
using TaskFlow.Application.Features.Boards.Commands.UpdateBoard;
using TaskFlow.Application.Features.Boards.Commands.DeleteBoard;
using TaskFlow.Application.Features.Boards.Queries.GetBoardById;
using TaskFlow.Application.Features.Boards.Queries.GetBoardsByOwnerId;
using TaskFlow.Application.Features.Boards.Queries.GetAllBoards;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tüm boardları sayfalı olarak getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllBoardsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// ID'ye göre board getir (detaylı, task'larla birlikte)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetBoardByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Kullanıcının boardlarını getir
    /// </summary>
    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetByOwnerId(int ownerId)
    {
        var query = new GetBoardsByOwnerIdQuery { OwnerId = ownerId };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Yeni board oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoardCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    /// <summary>
    /// Board bilgilerini güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBoardCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// Board'u sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteBoardCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}
