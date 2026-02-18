using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class Notification : BaseEntity
{
    public int UserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public NotificationType Type { get; private set; }
    public int? RelatedEntityId { get; private set; }
    public bool IsRead { get; private set; }

    // Navigation Properties
    public User User { get; private set; }

    private Notification() { }

    public Notification(int userId, string title, string message, NotificationType type, int? relatedEntityId = null)
    {
        UserId = userId;
        Title = title;
        Message = message;
        Type = type;
        RelatedEntityId = relatedEntityId;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
