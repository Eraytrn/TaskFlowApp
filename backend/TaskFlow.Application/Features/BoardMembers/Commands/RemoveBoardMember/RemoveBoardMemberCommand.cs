using MediatR;

namespace TaskFlow.Application.Features.BoardMembers.Commands.RemoveBoardMember;

public class RemoveBoardMemberCommand : IRequest<Unit>
{
    public int BoardId { get; set; }
    public int UserId { get; set; }
}
