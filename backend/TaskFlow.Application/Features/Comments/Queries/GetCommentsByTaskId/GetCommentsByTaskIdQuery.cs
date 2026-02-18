using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Comments.DTOs;

namespace TaskFlow.Application.Features.Comments.Queries.GetCommentsByTaskId;

public class GetCommentsByTaskIdQuery : IRequest<Result<List<CommentDto>>>
{
    public int TaskItemId { get; set; }
}
