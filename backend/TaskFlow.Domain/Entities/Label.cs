using TaskFlow.Domain.Common;

namespace TaskFlow.Domain.Entities;

public class Label : BaseEntity
{
    public int BoardId { get; set; }
    public string Name { get; set; }
    public string ColorHex { get; set; }

    // Navigation
    public Board Board { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    private Label() { }

    public Label(int boardId, string name, string colorHex)
    {
        BoardId = boardId;
        Name = name;
        ColorHex = colorHex;
    }
}
