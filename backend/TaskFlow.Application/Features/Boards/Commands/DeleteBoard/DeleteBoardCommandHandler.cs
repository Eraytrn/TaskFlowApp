using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteBoard;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBoardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        // Board'u bul
        var board = await _unitOfWork.Boards.GetByIdAsync(request.Id, cancellationToken);
        if (board == null)
        {
            return Result<bool>.Failure($"Board with ID {request.Id} not found.");
        }

        // Sil
        await _unitOfWork.Boards.DeleteAsync(board);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}