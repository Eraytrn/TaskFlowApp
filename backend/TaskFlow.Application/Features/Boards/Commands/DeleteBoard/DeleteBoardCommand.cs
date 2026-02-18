using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.Boards.Commands.DeleteBoard;

public class DeleteBoardCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}