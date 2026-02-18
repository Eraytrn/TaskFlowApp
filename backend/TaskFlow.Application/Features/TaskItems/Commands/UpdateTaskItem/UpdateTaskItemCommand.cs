using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Commands.UpdateTaskItem;

public class UpdateTaskItemCommand : IRequest<Result<TaskItemDto>>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}