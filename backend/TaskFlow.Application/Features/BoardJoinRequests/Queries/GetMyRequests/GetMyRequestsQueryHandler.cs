using AutoMapper;
using MediatR;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.BoardJoinRequests.Queries.GetMyRequests;

public class GetMyRequestsQueryHandler : IRequestHandler<GetMyRequestsQuery, IEnumerable<BoardJoinRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BoardJoinRequestDto>> Handle(GetMyRequestsQuery request, CancellationToken cancellationToken)
    {
        var requests = await _unitOfWork.JoinRequests.GetRequestsByUserIdAsync(request.UserId, cancellationToken);
        return _mapper.Map<IEnumerable<BoardJoinRequestDto>>(requests);
    }
}
