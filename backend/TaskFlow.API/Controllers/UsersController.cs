using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Users.Commands.CreateUser;
using TaskFlow.Application.Features.Users.Commands.UpdateUser;
using TaskFlow.Application.Features.Users.Commands.DeleteUser;
using TaskFlow.Application.Features.Users.Queries.GetUserById;
using TaskFlow.Application.Features.Users.Queries.GetAllUsers;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tüm kullanıcıları sayfalı olarak getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// ID'ye göre kullanıcı getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return Ok(result.Data);
    }

    /// <summary>
    /// Yeni kullanıcı oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    /// <summary>
    /// Kullanıcı bilgilerini güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors, message = result.ErrorMessage });

        return Ok(result.Data);
    }

    /// <summary>
    /// Kullanıcıyı sil
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteUserCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}
