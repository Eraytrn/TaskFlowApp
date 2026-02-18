namespace TaskFlow.Application.Features.TaskItems.DTOs;

public class TaskItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty; // "Todo", "InProgress", "Done"
    public int BoardId { get; set; }
    public string BoardName { get; set; } = string.Empty;
    public int? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public ICollection<TaskFlow.Application.Features.Labels.DTOs.LabelDto> Labels { get; set; } = new List<TaskFlow.Application.Features.Labels.DTOs.LabelDto>();
}
