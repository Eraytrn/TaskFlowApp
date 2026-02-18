using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Features.BoardMembers.Queries.GetBoardMembers;

public class GetBoardMembersQuery : IRequest<IEnumerable<BoardMemberDto>>
{
    public int BoardId { get; set; }
}
