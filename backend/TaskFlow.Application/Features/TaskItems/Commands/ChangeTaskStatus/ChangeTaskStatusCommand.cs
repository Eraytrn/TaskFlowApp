using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;


namespace TaskFlow.Application.Features.TaskItems.Commands.ChangeTaskStatus;

public class ChangeTaskStatusCommand : IRequest<Result<TaskItemDto>>
{
    public int Id { get; set; }
    public Domain.Enums.TaskStatus NewStatus { get; set; }
}
