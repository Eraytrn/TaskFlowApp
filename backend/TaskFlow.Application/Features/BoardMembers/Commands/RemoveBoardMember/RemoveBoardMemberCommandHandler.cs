using MediatR;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardMembers.Commands.RemoveBoardMember;

public class RemoveBoardMemberCommandHandler : IRequestHandler<RemoveBoardMemberCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBoardMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RemoveBoardMemberCommand request, CancellationToken cancellationToken)
    {
        var membership = await _unitOfWork.BoardMembers.GetMembershipAsync(request.BoardId, request.UserId, cancellationToken);
        
        if (membership == null)
            throw new KeyNotFoundException("Membership not found.");

        // Cannot remove owner
        if (membership.Role == BoardMemberRole.Owner)
            throw new InvalidOperationException("Cannot remove the board owner.");

        await _unitOfWork.BoardMembers.DeleteAsync(membership);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
