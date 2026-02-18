using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Application.Common.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemsByBoardId;

public class GetTaskItemsByBoardIdQueryHandler : IRequestHandler<GetTaskItemsByBoardIdQuery, Result<List<TaskItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetTaskItemsByBoardIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<TaskItemDto>>> Handle(GetTaskItemsByBoardIdQuery request, CancellationToken cancellationToken)
    {
        // Board'u üyeleri ve task'ları ile birlikte getir
        // GetBoardWithTasksAsync includes Members (added in previous step) and Tasks
        var board = await _unitOfWork.Boards.GetBoardWithTasksAsync(request.BoardId, cancellationToken);
        
        if (board == null)
        {
            return Result<List<TaskItemDto>>.Failure($"Board with ID {request.BoardId} not found.");
        }

        // Access Control
        var currentUserId = _currentUserService.UserId;
        var isMember = board.Members.Any(m => m.UserId == currentUserId);
        
        if (board.OwnerId != currentUserId && !isMember)
        {
             return Result<List<TaskItemDto>>.Failure("You do not have access to this board's tasks.");
        }

        // DTO'ya çevir
        var taskItemDtos = _mapper.Map<List<TaskItemDto>>(board.Tasks);

        return Result<List<TaskItemDto>>.Success(taskItemDtos);
    }
}
