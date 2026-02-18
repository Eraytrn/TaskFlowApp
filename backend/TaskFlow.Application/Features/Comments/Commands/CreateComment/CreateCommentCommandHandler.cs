using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Comments.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        // TaskItem var mı kontrol et
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.TaskItemId, cancellationToken);
        if (taskItem == null)
        {
            return Result<CommentDto>.Failure($"TaskItem with ID {request.TaskItemId} not found.");
        }

        // User var mı kontrol et
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<CommentDto>.Failure($"User with ID {request.UserId} not found.");
        }

        // Comment oluştur
        var comment = new Comment(request.Content, request.TaskItemId, request.UserId);

        // Veritabanına ekle
        await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Comment'i User ile birlikte tekrar getir
        var comments = await _unitOfWork.Comments.GetCommentsByTaskItemIdAsync(request.TaskItemId, cancellationToken);
        var savedComment = comments.FirstOrDefault(c => c.Id == comment.Id);

        // DTO'ya çevir
        var commentDto = _mapper.Map<CommentDto>(savedComment);

        return Result<CommentDto>.Success(commentDto);
    }
}
