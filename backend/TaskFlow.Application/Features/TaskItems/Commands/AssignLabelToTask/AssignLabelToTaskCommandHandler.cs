using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.TaskItems.Commands.AssignLabelToTask;

public class AssignLabelToTaskCommandHandler : IRequestHandler<AssignLabelToTaskCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignLabelToTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AssignLabelToTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.TaskItems.GetTaskWithCommentsAsync(request.TaskId, cancellationToken);
        if (task == null)
            return Result.Failure($"Task with ID {request.TaskId} not found.");

        var label = await _unitOfWork.Labels.GetByIdAsync(request.LabelId, cancellationToken);
        if (label == null)
            return Result.Failure($"Label with ID {request.LabelId} not found.");

        task.AddLabel(label);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
