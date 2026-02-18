using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Features.Notifications.Commands.MarkAsRead;
using TaskFlow.Application.Features.Notifications.Queries.GetUnreadNotifications;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get unread notifications for current user
    /// </summary>
    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUnreadNotificationsQuery { UserId = userId };
        var notifications = await _mediator.Send(query);
        return Ok(notifications);
    }

    /// <summary>
    /// Mark a notification as read
    /// </summary>
    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var command = new MarkAsReadCommand { NotificationId = id };
        await _mediator.Send(command);
        return Ok(new { message = "Notification marked as read" });
    }
}
