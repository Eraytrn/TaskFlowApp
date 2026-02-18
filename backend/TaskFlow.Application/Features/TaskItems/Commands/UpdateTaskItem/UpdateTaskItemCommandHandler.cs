using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Commands.UpdateTaskItem;

public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, Result<TaskItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTaskItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TaskItemDto>> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        // TaskItem'ı bul
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.Id, cancellationToken);
        if (taskItem == null)
        {
            return Result<TaskItemDto>.Failure($"TaskItem with ID {request.Id} not found.");
        }

        // Domain metoduyla güncelle
        taskItem.UpdateDetails(request.Title, request.Description);

        // Veritabanına kaydet
        await _unitOfWork.TaskItems.UpdateAsync(taskItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TaskItem'ı Board ve User ile birlikte tekrar getir
        var taskItems = await _unitOfWork.TaskItems.GetTasksByBoardIdAsync(taskItem.BoardId, cancellationToken);
        var updatedTaskItem = taskItems.FirstOrDefault(t => t.Id == request.Id);

        // DTO'ya çevir (AutoMapper otomatik doldurur)
        var taskItemDto = _mapper.Map<TaskItemDto>(updatedTaskItem);

        return Result<TaskItemDto>.Success(taskItemDto);
    }
}
