using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Enums.TaskStatus Status { get; private set; }
    public int BoardId { get; private set; }
    public int? AssignedUserId { get; private set; }



    // Navigation Properties
    public Board Board { get; private set; }
    public User AssignedUser { get; private set; }
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    public ICollection<Label> Labels { get; private set; } = new List<Label>();

    // db den taskları dondurmek ıcın bu constructor kullanılır, private olma nedeni dısarıdan birinin bos param lı nesne gondermesini engellemek
    private TaskItem() { }

    public TaskItem(string title, string description, int boardId)
    {
        Title = title;
        Description = description;
        BoardId = boardId;
        Status = Enums.TaskStatus.Todo;
    }

    public void ChangeStatus(Enums.TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedDate = DateTime.UtcNow;
    }

    public void AssignToUser(int userId)
    {
        AssignedUserId = userId;
        UpdatedDate = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description)
    {
        Title = title;
        Description = description;
        UpdatedDate = DateTime.UtcNow;
    }

    public void AddLabel(Label label)
    {
        if (!Labels.Any(l => l.Id == label.Id))
        {
            Labels.Add(label);
            UpdatedDate = DateTime.UtcNow;
        }
    }

    public void RemoveLabel(int labelId)
    {
        var label = Labels.FirstOrDefault(l => l.Id == labelId);
        if (label != null)
        {
            Labels.Remove(label);
            UpdatedDate = DateTime.UtcNow;
        }
    }
}

