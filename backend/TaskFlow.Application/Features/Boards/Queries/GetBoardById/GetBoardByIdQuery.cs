using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Boards.DTOs;

namespace TaskFlow.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdQuery : IRequest<Result<BoardDetailDto>>
{
    public int Id { get; set; }
}