using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Users.DTOs;

namespace TaskFlow.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public int Id { get; set; }
}
