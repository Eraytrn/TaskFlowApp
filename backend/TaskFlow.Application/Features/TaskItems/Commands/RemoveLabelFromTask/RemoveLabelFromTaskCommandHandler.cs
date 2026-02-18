using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.TaskItems.Commands.RemoveLabelFromTask;

public class RemoveLabelFromTaskCommandHandler : IRequestHandler<RemoveLabelFromTaskCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveLabelFromTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveLabelFromTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _unitOfWork.TaskItems.GetTaskWithCommentsAsync(request.TaskId, cancellationToken);
        if (task == null)
            return Result.Failure($"Task with ID {request.TaskId} not found.");

        task.RemoveLabel(request.LabelId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
