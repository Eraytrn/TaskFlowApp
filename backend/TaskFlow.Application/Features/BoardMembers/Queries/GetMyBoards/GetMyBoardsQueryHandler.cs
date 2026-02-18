using AutoMapper;
using MediatR;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardMembers.Queries.GetMyBoards;

public class GetMyBoardsQueryHandler : IRequestHandler<GetMyBoardsQuery, IEnumerable<BoardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyBoardsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BoardDto>> Handle(GetMyBoardsQuery request, CancellationToken cancellationToken)
    {
        var memberships = await _unitOfWork.BoardMembers.GetBoardsByUserIdAsync(request.UserId, cancellationToken);
        var boards = memberships.Select(m => m.Board);
        return _mapper.Map<IEnumerable<BoardDto>>(boards);
    }
}
