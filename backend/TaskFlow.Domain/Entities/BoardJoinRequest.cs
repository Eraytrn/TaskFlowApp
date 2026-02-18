using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class BoardJoinRequest : BaseEntity
{
    public int BoardId { get; private set; }
    public int RequesterId { get; private set; }
    public RequestStatus Status { get; private set; }
    public DateTime RequestDate { get; private set; }
    public DateTime? ResponseDate { get; private set; }
    public int? ResponderId { get; private set; }

    // Navigation Properties
    public Board Board { get; private set; }
    public User Requester { get; private set; }
    public User Responder { get; private set; }

    private BoardJoinRequest() { }

    public BoardJoinRequest(int boardId, int requesterId)
    {
        BoardId = boardId;
        RequesterId = requesterId;
        Status = RequestStatus.Pending;
        RequestDate = DateTime.UtcNow;
    }

    public void Approve(int responderId)
    {
        if (Status != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved.");

        Status = RequestStatus.Approved;
        ResponderId = responderId;
        ResponseDate = DateTime.UtcNow;
    }

    public void Reject(int responderId)
    {
        if (Status != RequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be rejected.");

        Status = RequestStatus.Rejected;
        ResponderId = responderId;
        ResponseDate = DateTime.UtcNow;
    }
}
