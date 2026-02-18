using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemsByUserId;

public class GetTaskItemsByUserIdQuery : IRequest<Result<List<TaskItemDto>>>
{
    public int UserId { get; set; }
}
