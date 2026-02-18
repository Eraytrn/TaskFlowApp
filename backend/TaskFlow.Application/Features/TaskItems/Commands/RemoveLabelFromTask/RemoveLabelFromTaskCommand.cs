using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.TaskItems.Commands.RemoveLabelFromTask;

public class RemoveLabelFromTaskCommand : IRequest<Result>
{
    public int TaskId { get; set; }
    public int LabelId { get; set; }
}
