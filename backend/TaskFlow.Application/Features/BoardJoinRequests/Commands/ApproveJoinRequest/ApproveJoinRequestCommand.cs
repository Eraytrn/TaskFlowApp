using MediatR;

namespace TaskFlow.Application.Features.BoardJoinRequests.Commands.ApproveJoinRequest;

public class ApproveJoinRequestCommand : IRequest<Unit>
{
    public int RequestId { get; set; }
    public int ResponderId { get; set; }
}
