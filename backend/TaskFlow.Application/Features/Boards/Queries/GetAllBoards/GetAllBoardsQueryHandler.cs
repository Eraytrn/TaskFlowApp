using AutoMapper;
using MediatR;
using System.Linq;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Boards.Queries.GetAllBoards;

public class GetAllBoardsQueryHandler : IRequestHandler<GetAllBoardsQuery, Result<PaginatedList<BoardDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBoardsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<BoardDto>>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
    {
        // Tüm boardları getir
        var boards = await _unitOfWork.Boards.GetAllAsync(cancellationToken);

        // DTO'ya çevir
        var boardDtos = _mapper.Map<List<BoardDto>>(boards);

        // Owner adlarını getir (her board için)
        var ownerIds = boardDtos.Select(b => b.OwnerId).Distinct().ToList();
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
        var ownerDict = users.Where(u => ownerIds.Contains(u.Id))
                             .ToDictionary(u => u.Id, u => u.FullName);

        foreach (var boardDto in boardDtos)
        {
            boardDto.OwnerName = ownerDict.ContainsKey(boardDto.OwnerId)
                ? ownerDict[boardDto.OwnerId]
                : "Unknown";
        }

        // Paginate et
        var paginatedBoards = PaginatedList<BoardDto>.Create(
            boardDtos.AsQueryable(),
            request.PageNumber,
            request.PageSize
        );

        return Result<PaginatedList<BoardDto>>.Success(paginatedBoards);
    }
}