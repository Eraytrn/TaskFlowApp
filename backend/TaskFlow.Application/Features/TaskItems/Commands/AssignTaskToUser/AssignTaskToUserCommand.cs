using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Commands.AssignTaskToUser;

public class AssignTaskToUserCommand : IRequest<Result<TaskItemDto>>
{
    public int TaskItemId { get; set; }
    public int UserId { get; set; }
}
