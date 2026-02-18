using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Features.BoardMembers.Commands.AddBoardMember;
using TaskFlow.Application.Features.BoardMembers.Commands.RemoveBoardMember;
using TaskFlow.Application.Features.BoardMembers.Queries.GetBoardMembers;
using TaskFlow.Application.Features.BoardMembers.Queries.GetMyBoards;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardMembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardMembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all members of a board
    /// </summary>
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetBoardMembers(int boardId)
    {
        var query = new GetBoardMembersQuery { BoardId = boardId };
        var members = await _mediator.Send(query);
        return Ok(members);
    }

    /// <summary>
    /// Get boards where current user is owner or member
    /// </summary>
    [HttpGet("my-boards")]
    public async Task<IActionResult> GetMyBoards()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetMyBoardsQuery { UserId = userId };
        var boards = await _mediator.Send(query);
        return Ok(boards);
    }

    /// <summary>
    /// Add a member to a board (owner only)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddMember([FromBody] AddBoardMemberCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Member added successfully" });
    }

    /// <summary>
    /// Remove a member from a board (owner only)
    /// </summary>
    [HttpDelete("{boardId}/user/{userId}")]
    public async Task<IActionResult> RemoveMember(int boardId, int userId)
    {
        var command = new RemoveBoardMemberCommand { BoardId = boardId, UserId = userId };
        await _mediator.Send(command);
        return Ok(new { message = "Member removed successfully" });
    }
}
