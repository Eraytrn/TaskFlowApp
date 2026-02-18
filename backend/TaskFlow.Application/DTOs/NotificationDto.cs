using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public NotificationType Type { get; set; }
    public int? RelatedEntityId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
}
