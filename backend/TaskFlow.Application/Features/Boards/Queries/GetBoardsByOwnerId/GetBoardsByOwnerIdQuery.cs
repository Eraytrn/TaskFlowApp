using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardsByOwnerId;

public class GetBoardsByOwnerIdQuery : IRequest<Result<List<BoardDto>>>
{
    public int OwnerId { get; set; }
}