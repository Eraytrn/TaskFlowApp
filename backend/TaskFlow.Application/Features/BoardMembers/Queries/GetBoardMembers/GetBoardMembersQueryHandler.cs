using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardMembers.Queries.GetBoardMembers;

public class GetBoardMembersQueryHandler : IRequestHandler<GetBoardMembersQuery, IEnumerable<BoardMemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBoardMembersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BoardMemberDto>> Handle(GetBoardMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _unitOfWork.BoardMembers.GetMembersByBoardIdAsync(request.BoardId, cancellationToken);
        return _mapper.Map<IEnumerable<BoardMemberDto>>(members);
    }
}
