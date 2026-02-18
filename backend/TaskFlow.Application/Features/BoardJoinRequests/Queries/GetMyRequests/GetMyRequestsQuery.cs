using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Features.BoardJoinRequests.Queries.GetMyRequests;

public class GetMyRequestsQuery : IRequest<IEnumerable<BoardJoinRequestDto>>
{
    public int UserId { get; set; }
}
