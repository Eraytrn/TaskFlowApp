using MediatR;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Features.BoardJoinRequests.Commands.CreateJoinRequest;

public class CreateJoinRequestCommand : IRequest<BoardJoinRequestDto>
{
    public int BoardId { get; set; }
    public int RequesterId { get; set; }
}
