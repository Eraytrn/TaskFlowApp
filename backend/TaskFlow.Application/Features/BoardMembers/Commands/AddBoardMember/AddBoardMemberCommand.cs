using MediatR;

namespace TaskFlow.Application.Features.BoardMembers.Commands.AddBoardMember;

public class AddBoardMemberCommand : IRequest<Unit>
{
    public int BoardId { get; set; }
    public int UserId { get; set; }
}
