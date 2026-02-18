using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Auth.DTOs;

namespace TaskFlow.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
