using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Boards.Commands.UpdateBoard;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, Result<BoardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBoardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BoardDto>> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        // Board'u bul
        var board = await _unitOfWork.Boards.GetByIdAsync(request.Id, cancellationToken);
        if (board == null)
        {
            return Result<BoardDto>.Failure($"Board with ID {request.Id} not found.");
        }

        // Board'u güncelle (Domain metodu kullanarak)
        board.UpdateDetails(request.Title, request.Description);

        // Veritabanına kaydet
        await _unitOfWork.Boards.UpdateAsync(board);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Owner bilgisini getir
        var owner = await _unitOfWork.Users.GetByIdAsync(board.OwnerId, cancellationToken);

        // DTO'ya çevir
        var boardDto = _mapper.Map<BoardDto>(board);
        boardDto.OwnerName = owner?.FullName ?? "Unknown";

        return Result<BoardDto>.Success(boardDto);
    }
}