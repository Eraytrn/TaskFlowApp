using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.TaskItems.Commands.AssignLabelToTask;

public class AssignLabelToTaskCommand : IRequest<Result>
{
    public int TaskId { get; set; }
    public int LabelId { get; set; }
}
