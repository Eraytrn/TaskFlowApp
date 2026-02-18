using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Comments.DTOs;

namespace TaskFlow.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentCommand : IRequest<Result<CommentDto>>
{
    public string Content { get; set; } = string.Empty;
    public int TaskItemId { get; set; }
    public int UserId { get; set; }
}
