using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Users.DTOs;

namespace TaskFlow.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<Result<List<UserDto>>>
{
    // Pagination parametreleri
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
