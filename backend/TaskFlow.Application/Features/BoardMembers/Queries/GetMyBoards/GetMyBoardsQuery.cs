using MediatR;
using TaskFlow.Application.Features.Boards.DTOs;

namespace TaskFlow.Application.Features.BoardMembers.Queries.GetMyBoards;

public class GetMyBoardsQuery : IRequest<IEnumerable<BoardDto>>
{
    public int UserId { get; set; }
}
