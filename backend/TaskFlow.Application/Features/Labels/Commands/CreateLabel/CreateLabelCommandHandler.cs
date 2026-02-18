using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Labels.Commands.CreateLabel;

public class CreateLabelCommandHandler : IRequestHandler<CreateLabelCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateLabelCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateLabelCommand request, CancellationToken cancellationToken)
    {
        // Board existence check could be here or authorized via policy
        var board = await _unitOfWork.Boards.GetByIdAsync(request.BoardId, cancellationToken);
        if (board == null)
            return Result<int>.Failure($"Board with ID {request.BoardId} not found.");

        var label = new Label(request.BoardId, request.Name, request.ColorHex);

        await _unitOfWork.Labels.AddAsync(label, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(label.Id);
    }
}
