using MediatR;

namespace TaskFlow.Application.Features.BoardJoinRequests.Commands.RejectJoinRequest;

public class RejectJoinRequestCommand : IRequest<Unit>
{
    public int RequestId { get; set; }
    public int ResponderId { get; set; }
}
