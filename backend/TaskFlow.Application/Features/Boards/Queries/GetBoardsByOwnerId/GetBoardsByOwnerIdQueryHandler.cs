using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardsByOwnerId;

public class GetBoardsByOwnerIdQueryHandler : IRequestHandler<GetBoardsByOwnerIdQuery, Result<List<BoardDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBoardsByOwnerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<BoardDto>>> Handle(GetBoardsByOwnerIdQuery request, CancellationToken cancellationToken)
    {
        // Owner'ın var olup olmadığını kontrol et
        var owner = await _unitOfWork.Users.GetByIdAsync(request.OwnerId, cancellationToken);
        if (owner == null)
        {
            return Result<List<BoardDto>>.Failure($"User with ID {request.OwnerId} not found.");
        }

        // Owner'a ait boardları getir
        var boards = await _unitOfWork.Boards.GetBoardsByOwnerIdAsync(request.OwnerId, cancellationToken);

        // DTO'ya çevir
        var boardDtos = _mapper.Map<List<BoardDto>>(boards);

        // Her board için owner adını set et
        foreach (var boardDto in boardDtos)
        {
            boardDto.OwnerName = owner.FullName;
        }

        return Result<List<BoardDto>>.Success(boardDtos);
    }
}