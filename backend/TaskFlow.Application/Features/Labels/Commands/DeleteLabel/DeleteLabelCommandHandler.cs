using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Labels.Commands.DeleteLabel;

public class DeleteLabelCommandHandler : IRequestHandler<DeleteLabelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLabelCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {
        var label = await _unitOfWork.Labels.GetByIdAsync(request.Id, cancellationToken);
        if (label == null)
            return Result.Failure($"Label with ID {request.Id} not found.");

        await _unitOfWork.Labels.DeleteAsync(label);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
