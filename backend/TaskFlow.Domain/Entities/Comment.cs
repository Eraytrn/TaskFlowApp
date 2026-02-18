using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; private set; }
    public int TaskItemId { get; private set; }
    public int UserId { get; private set; }

    // Navigation Properties
    public TaskItem TaskItem { get; private set; }
    public User User { get; private set; }

    private Comment() { }

    public Comment(string content, int taskItemId, int userId)
    {
        Content = content;
        TaskItemId = taskItemId;
        UserId = userId;
    }
}
