using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Auth.DTOs;

namespace TaskFlow.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<Result<AuthResponseDto>>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
