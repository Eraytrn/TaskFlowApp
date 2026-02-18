using MediatR;

namespace TaskFlow.Application.Features.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommand : IRequest<Unit>
{
    public int NotificationId { get; set; }
}
