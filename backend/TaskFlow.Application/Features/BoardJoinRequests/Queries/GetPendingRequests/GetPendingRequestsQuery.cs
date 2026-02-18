using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Features.BoardJoinRequests.Queries.GetPendingRequests;

public class GetPendingRequestsQuery : IRequest<IEnumerable<BoardJoinRequestDto>>
{
    public int BoardId { get; set; }
}
