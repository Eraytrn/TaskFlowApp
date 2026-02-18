using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Commands.AssignTaskToUser;

public class AssignTaskToUserCommandHandler : IRequestHandler<AssignTaskToUserCommand, Result<TaskItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssignTaskToUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TaskItemDto>> Handle(AssignTaskToUserCommand request, CancellationToken cancellationToken)
    {
        // TaskItem'ı bul
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.TaskItemId, cancellationToken);
        if (taskItem == null)
        {
            return Result<TaskItemDto>.Failure($"TaskItem with ID {request.TaskItemId} not found.");
        }

        // User'ı bul
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<TaskItemDto>.Failure($"User with ID {request.UserId} not found.");
        }

        // Task'ı kullanıcıya ata (Domain metodu)
        taskItem.AssignToUser(request.UserId);

        // Veritabanına kaydet
        await _unitOfWork.TaskItems.UpdateAsync(taskItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TaskItem'ı Board ve User ile birlikte tekrar getir
        var taskItems = await _unitOfWork.TaskItems.GetTasksByBoardIdAsync(taskItem.BoardId, cancellationToken);
        var updatedTaskItem = taskItems.FirstOrDefault(t => t.Id == request.TaskItemId);

        // DTO'ya çevir (AutoMapper otomatik doldurur)
        var taskItemDto = _mapper.Map<TaskItemDto>(updatedTaskItem);

        return Result<TaskItemDto>.Success(taskItemDto);
    }
}
