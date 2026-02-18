using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Result<BoardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBoardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BoardDto>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        // Owner'ın var olup olmadığını kontrol et
        var owner = await _unitOfWork.Users.GetByIdAsync(request.OwnerId, cancellationToken);
        if (owner == null)
        {
            return Result<BoardDto>.Failure($"Owner with ID {request.OwnerId} not found.");
        }

        // Board oluştur
        var board = new Board(request.Title, request.Description, request.OwnerId);

        // Veritabanına ekle
        await _unitOfWork.Boards.AddAsync(board, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Owner'ı otomatik olarak BoardMember olarak ekle
        var boardMember = new BoardMember(board.Id, request.OwnerId, BoardMemberRole.Owner);
        await _unitOfWork.BoardMembers.AddAsync(boardMember, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // DTO'ya çevir
        var boardDto = _mapper.Map<BoardDto>(board);
        boardDto.OwnerName = owner.FullName; // Owner adını manuel set ediyoruz

        return Result<BoardDto>.Success(boardDto);
    }
}