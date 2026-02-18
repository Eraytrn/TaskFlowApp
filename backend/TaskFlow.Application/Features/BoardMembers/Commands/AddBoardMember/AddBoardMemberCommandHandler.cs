using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardMembers.Commands.AddBoardMember;

public class AddBoardMemberCommandHandler : IRequestHandler<AddBoardMemberCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddBoardMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddBoardMemberCommand request, CancellationToken cancellationToken)
    {
        // Check if board exists
        var board = await _unitOfWork.Boards.GetByIdAsync(request.BoardId, cancellationToken);
        if (board == null)
            throw new KeyNotFoundException($"Board with ID {request.BoardId} not found.");

        // Check if user exists
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

        // Check if already a member
        var existingMembership = await _unitOfWork.BoardMembers.GetMembershipAsync(request.BoardId, request.UserId, cancellationToken);
        if (existingMembership != null)
            throw new InvalidOperationException("User is already a member of this board.");

        // Add as member
        var boardMember = new BoardMember(request.BoardId, request.UserId, BoardMemberRole.Member);
        await _unitOfWork.BoardMembers.AddAsync(boardMember, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
