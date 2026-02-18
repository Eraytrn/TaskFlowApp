using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Features.Notifications.Queries.GetUnreadNotifications;

public class GetUnreadNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
{
    public int UserId { get; set; }
}
