using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Users.DTOs;

namespace TaskFlow.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// Yeni kullanıcı oluşturma command'i
/// MediatR IRequest<TResponse> implement eder
/// </summary>
public class CreateUserCommand : IRequest<Result<UserDto>>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
