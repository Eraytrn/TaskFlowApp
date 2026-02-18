using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}
