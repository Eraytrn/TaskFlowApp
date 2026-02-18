using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
