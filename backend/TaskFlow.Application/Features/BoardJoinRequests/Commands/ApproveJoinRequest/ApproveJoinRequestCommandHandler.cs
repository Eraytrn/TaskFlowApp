using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardJoinRequests.Commands.ApproveJoinRequest;

public class ApproveJoinRequestCommandHandler : IRequestHandler<ApproveJoinRequestCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveJoinRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ApproveJoinRequestCommand request, CancellationToken cancellationToken)
    {
        var joinRequest = await _unitOfWork.JoinRequests.GetByIdAsync(request.RequestId, cancellationToken);
        if (joinRequest == null)
            throw new KeyNotFoundException($"Join request with ID {request.RequestId} not found.");

        var board = await _unitOfWork.Boards.GetByIdAsync(joinRequest.BoardId, cancellationToken);
        if (board == null)
            throw new KeyNotFoundException($"The board associated with this request (ID: {joinRequest.BoardId}) no longer exists.");

        // Approve the request
        joinRequest.Approve(request.ResponderId);

        // Add user as board member
        var boardMember = new BoardMember(joinRequest.BoardId, joinRequest.RequesterId, BoardMemberRole.Member);
        await _unitOfWork.BoardMembers.AddAsync(boardMember, cancellationToken);

        // Create notification for requester
        var notification = new Notification(
            joinRequest.RequesterId,
            "Join Request Approved",
            $"Your request to join board '{board.Title}' was approved!",
            NotificationType.RequestApproved,
            joinRequest.Id
        );
        await _unitOfWork.Notifications.AddAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
