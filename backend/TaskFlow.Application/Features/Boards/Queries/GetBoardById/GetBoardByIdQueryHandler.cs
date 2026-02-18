using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Common.Interfaces;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, Result<BoardDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetBoardByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BoardDetailDto>> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        // Board'u task'ları ile birlikte getir
        var board = await _unitOfWork.Boards.GetBoardWithTasksAsync(request.Id, cancellationToken);

        if (board == null)
        {
            return Result<BoardDetailDto>.Failure($"Board with ID {request.Id} not found.");
        }

        // Access Control: Check if user is Owner or Member
        var currentUserId = _currentUserService.UserId;
        var isMember = board.Members.Any(m => m.UserId == currentUserId);
        
        if (board.OwnerId != currentUserId && !isMember)
        {
             return Result<BoardDetailDto>.Failure("You do not have access to this board.");
        }

        // Owner bilgisini getir
        var owner = await _unitOfWork.Users.GetByIdAsync(board.OwnerId, cancellationToken);

        // DTO'ya çevir
        var boardDto = _mapper.Map<BoardDetailDto>(board);
        boardDto.OwnerName = owner?.FullName ?? "Unknown";

        // İstatistikleri hesapla
        boardDto.TotalTaskCount = board.Tasks.Count;
        boardDto.CompletedTaskCount = board.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Done);

        return Result<BoardDetailDto>.Success(boardDto);
    }
}