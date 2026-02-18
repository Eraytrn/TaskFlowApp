using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Comments.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Comments.Queries.GetCommentsByTaskId;

public class GetCommentsByTaskIdQueryHandler : IRequestHandler<GetCommentsByTaskIdQuery, Result<List<CommentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCommentsByTaskIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<CommentDto>>> Handle(GetCommentsByTaskIdQuery request, CancellationToken cancellationToken)
    {
        var comments = await _unitOfWork.Comments.GetCommentsByTaskItemIdAsync(request.TaskItemId, cancellationToken);
        var commentDtos = _mapper.Map<List<CommentDto>>(comments);

        return Result<List<CommentDto>>.Success(commentDtos);
    }
}
