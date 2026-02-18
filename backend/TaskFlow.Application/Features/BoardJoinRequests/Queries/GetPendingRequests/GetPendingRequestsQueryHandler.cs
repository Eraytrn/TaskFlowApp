using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardJoinRequests.Queries.GetPendingRequests;

public class GetPendingRequestsQueryHandler : IRequestHandler<GetPendingRequestsQuery, IEnumerable<BoardJoinRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPendingRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BoardJoinRequestDto>> Handle(GetPendingRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.JoinRequests.GetPendingRequestsByBoardIdAsync(request.BoardId, cancellationToken);
        return _mapper.Map<IEnumerable<BoardJoinRequestDto>>(requests);
    }
}
