using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Users.DTOs;

namespace TaskFlow.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result<UserDto>>
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
