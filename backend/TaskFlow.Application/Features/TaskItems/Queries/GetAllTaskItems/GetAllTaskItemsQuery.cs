using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetAllTaskItems;

public class GetAllTaskItemsQuery : IRequest<Result<PaginatedList<TaskItemDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
