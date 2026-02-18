using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommand : IRequest<Result<TaskItemDto>>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public int? AssignedUserId { get; set; } // Opsiyonel, task oluştururken kullanıcıya atanabilir
}