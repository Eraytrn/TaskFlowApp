using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemsByUserId;

public class GetTaskItemsByUserIdQueryHandler : IRequestHandler<GetTaskItemsByUserIdQuery, Result<List<TaskItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaskItemsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<TaskItemDto>>> Handle(GetTaskItemsByUserIdQuery request, CancellationToken cancellationToken)
    {
        // User'ın var olup olmadığını kontrol et
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<List<TaskItemDto>>.Failure($"User with ID {request.UserId} not found.");
        }

        // Kullanıcıya atanmış task'ları getir (Board ve User Include edilmiş)
        var taskItems = await _unitOfWork.TaskItems.GetTasksByAssignedUserIdAsync(request.UserId, cancellationToken);

        // DTO'ya çevir (AutoMapper otomatik doldurur)
        var taskItemDtos = _mapper.Map<List<TaskItemDto>>(taskItems);

        return Result<List<TaskItemDto>>.Success(taskItemDtos);
    }
}
