using TaskFlow.Application.Features.Comments.DTOs;

namespace TaskFlow.Application.Features.TaskItems.DTOs;

/// <summary>
/// Task detay sayfası için - Comment'lerle birlikte
/// </summary>
public class TaskItemDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public string BoardName { get; set; } = string.Empty;
    public int? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}
