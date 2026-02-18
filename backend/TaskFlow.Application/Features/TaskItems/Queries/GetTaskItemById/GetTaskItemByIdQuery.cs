using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemById;

public class GetTaskItemByIdQuery : IRequest<Result<TaskItemDetailDto>>
{
    public int Id { get; set; }
}
