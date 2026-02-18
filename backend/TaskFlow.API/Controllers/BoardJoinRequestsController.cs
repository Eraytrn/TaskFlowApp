using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Features.BoardJoinRequests.Commands.CreateJoinRequest;
using TaskFlow.Application.Features.BoardJoinRequests.Commands.ApproveJoinRequest;
using TaskFlow.Application.Features.BoardJoinRequests.Commands.RejectJoinRequest;
using TaskFlow.Application.Features.BoardJoinRequests.Queries.GetPendingRequests;
using TaskFlow.Application.Features.BoardJoinRequests.Queries.GetMyRequests;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardJoinRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardJoinRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get current user's join requests
    /// </summary>
    [HttpGet("my-requests")]
    public async Task<IActionResult> GetMyRequests()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetMyRequestsQuery { UserId = userId };
        var requests = await _mediator.Send(query);
        return Ok(requests);
    }

    /// <summary>
    /// Get pending join requests for a board (owner only)
    /// </summary>
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetPendingRequests(int boardId)
    {
        var query = new GetPendingRequestsQuery { BoardId = boardId };
        var requests = await _mediator.Send(query);
        return Ok(requests);
    }

    /// <summary>
    /// Create a join request
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromBody] CreateJoinRequestCommand command)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        command.RequesterId = userId;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Approve a join request (owner only)
    /// </summary>
    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> ApproveRequest(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new ApproveJoinRequestCommand { RequestId = id, ResponderId = userId };
        await _mediator.Send(command);
        return Ok(new { message = "Request approved" });
    }

    /// <summary>
    /// Reject a join request (owner only)
    /// </summary>
    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> RejectRequest(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new RejectJoinRequestCommand { RequestId = id, ResponderId = userId };
        await _mediator.Send(command);
        return Ok(new { message = "Request rejected" });
    }
}
