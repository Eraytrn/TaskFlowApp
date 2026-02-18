using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.TaskItems.Commands.DeleteTaskItem;

public class DeleteTaskItemCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
