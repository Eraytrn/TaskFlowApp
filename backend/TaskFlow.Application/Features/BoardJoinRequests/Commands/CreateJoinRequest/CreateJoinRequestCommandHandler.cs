using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardJoinRequests.Commands.CreateJoinRequest;

public class CreateJoinRequestCommandHandler : IRequestHandler<CreateJoinRequestCommand, BoardJoinRequestDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateJoinRequestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BoardJoinRequestDto> Handle(CreateJoinRequestCommand request, CancellationToken cancellationToken)
    {
        // Check if board exists
        var board = await _unitOfWork.Boards.GetByIdAsync(request.BoardId, cancellationToken);
        if (board == null)
            throw new KeyNotFoundException($"Board with ID {request.BoardId} not found.");

        // Check if already a member
        var isMember = await _unitOfWork.BoardMembers.IsMemberAsync(request.BoardId, request.RequesterId, cancellationToken);
        if (isMember)
            throw new InvalidOperationException("User is already a member of this board.");

        // Check if already has pending request
        var hasPending = await _unitOfWork.JoinRequests.HasPendingRequestAsync(request.BoardId, request.RequesterId, cancellationToken);
        if (hasPending)
            throw new InvalidOperationException("User already has a pending request for this board.");

        // Create join request
        var joinRequest = new BoardJoinRequest(request.BoardId, request.RequesterId);
        await _unitOfWork.JoinRequests.AddAsync(joinRequest, cancellationToken);
        
        // Save to get the ID
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch requester to get the name
        var requester = await _unitOfWork.Users.GetByIdAsync(request.RequesterId, cancellationToken);
        var requesterName = requester?.FullName ?? "Someone";

        // Create notification for board owner with the join request ID
        var notification = new Notification(
            board.OwnerId,
            "New Join Request",
            $"{requesterName} requested to join your board '{board.Title}'",
            NotificationType.JoinRequest,
            joinRequest.Id
        );
        await _unitOfWork.Notifications.AddAsync(notification, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with navigation properties for DTO mapping
        var createdRequest = await _unitOfWork.JoinRequests.GetByIdAsync(joinRequest.Id, cancellationToken);
        return _mapper.Map<BoardJoinRequestDto>(createdRequest);
    }
}
