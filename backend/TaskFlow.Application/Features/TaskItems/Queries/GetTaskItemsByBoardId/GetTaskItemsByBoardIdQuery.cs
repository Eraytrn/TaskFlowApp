using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemsByBoardId;

public class GetTaskItemsByBoardIdQuery : IRequest<Result<List<TaskItemDto>>>
{
    public int BoardId { get; set; }
}
