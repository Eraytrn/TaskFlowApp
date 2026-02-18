using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Application.Features.Labels.DTOs;
namespace TaskFlow.Application.Features.Boards.DTOs;

public class BoardDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    // Board'daki tüm tasklar
    public List<TaskItemDto> Tasks { get; set; } = new();

    // Board'daki tüm etiketler
    public List<LabelDto> Labels { get; set; } = new();
    
    // İstatistikler
    public int TotalTaskCount { get; set; }
    public int CompletedTaskCount { get; set; }
    
}