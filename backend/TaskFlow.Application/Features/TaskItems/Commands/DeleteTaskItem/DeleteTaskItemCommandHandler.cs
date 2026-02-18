using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Commands.DeleteTaskItem;

public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        // TaskItem'ı bul
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.Id, cancellationToken);
        if (taskItem == null)
        {
            return Result<bool>.Failure($"TaskItem with ID {request.Id} not found.");
        }

        // Sil (CASCADE ile comment'lar da silinir)
        await _unitOfWork.TaskItems.DeleteAsync(taskItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
