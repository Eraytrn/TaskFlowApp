using MediatR;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardJoinRequests.Commands.RejectJoinRequest;

public class RejectJoinRequestCommandHandler : IRequestHandler<RejectJoinRequestCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RejectJoinRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RejectJoinRequestCommand request, CancellationToken cancellationToken)
    {
        var joinRequest = await _unitOfWork.JoinRequests.GetByIdAsync(request.RequestId, cancellationToken);
        if (joinRequest == null)
            throw new KeyNotFoundException($"Join request with ID {request.RequestId} not found.");

        var board = await _unitOfWork.Boards.GetByIdAsync(joinRequest.BoardId, cancellationToken);
        if (board == null)
            throw new KeyNotFoundException($"The board associated with this request (ID: {joinRequest.BoardId}) no longer exists.");

        // Reject the request
        joinRequest.Reject(request.ResponderId);

        // Create notification for requester
        var notification = new Notification(
            joinRequest.RequesterId,
            "Join Request Rejected",
            $"Your request to join board '{board.Title}' was rejected.",
            NotificationType.RequestRejected,
            joinRequest.BoardId
        );
        await _unitOfWork.Notifications.AddAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
