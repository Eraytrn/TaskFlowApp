using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Application.Common.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, Result<TaskItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateTaskItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TaskItemDto>> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        // Board var mı kontrol et (Access check için Members yüklü olmalı)
        var board = await _unitOfWork.Boards.GetBoardWithTasksAsync(request.BoardId, cancellationToken);
        if (board == null)
        {
            return Result<TaskItemDto>.Failure($"Board with ID {request.BoardId} not found.");
        }

        // Access Control
        var currentUserId = _currentUserService.UserId;
        var isMember = board.Members.Any(m => m.UserId == currentUserId);
        
        if (board.OwnerId != currentUserId && !isMember)
        {
             return Result<TaskItemDto>.Failure("You do not have access to create tasks in this board.");
        }

        // Eğer AssignedUserId varsa, user'ın var olup olmadığını kontrol et
        if (request.AssignedUserId.HasValue)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.AssignedUserId.Value, cancellationToken);
            if (user == null)
            {
                return Result<TaskItemDto>.Failure($"User with ID {request.AssignedUserId.Value} not found.");
            }
        }

        // TaskItem oluştur
        var taskItem = new TaskItem(request.Title, request.Description, request.BoardId);

        // Eğer kullanıcı atanmışsa, ata
        if (request.AssignedUserId.HasValue)
        {
            taskItem.AssignToUser(request.AssignedUserId.Value);
        }

        // Veritabanına ekle
        await _unitOfWork.TaskItems.AddAsync(taskItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TaskItem'ı Board ve User ile birlikte tekrar getir
        var taskItems = await _unitOfWork.TaskItems.GetTasksByBoardIdAsync(request.BoardId, cancellationToken);
        var savedTaskItem = taskItems.FirstOrDefault(t => t.Id == taskItem.Id);

        // DTO'ya çevir (AutoMapper Board ve User bilgilerini otomatik alır)
        var taskItemDto = _mapper.Map<TaskItemDto>(savedTaskItem);

        return Result<TaskItemDto>.Success(taskItemDto);
    }
}
