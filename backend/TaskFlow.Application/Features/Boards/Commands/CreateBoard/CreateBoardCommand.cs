using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;

namespace TaskFlow.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommand : IRequest<Result<BoardDto>>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int OwnerId { get; set; }
}