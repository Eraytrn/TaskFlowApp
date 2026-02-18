using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetTaskItemById;

public class GetTaskItemByIdQueryHandler : IRequestHandler<GetTaskItemByIdQuery, Result<TaskItemDetailDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaskItemByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TaskItemDetailDto>> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken)
    {
        // TaskItem'ı comment'larıyla birlikte getir (Board ve User da Include edilmiş)
        var taskItem = await _unitOfWork.TaskItems.GetTaskWithCommentsAsync(request.Id, cancellationToken);
        if (taskItem == null)
        {
            return Result<TaskItemDetailDto>.Failure($"TaskItem with ID {request.Id} not found.");
        }

        // DTO'ya çevir (AutoMapper Board, User, Comments otomatik doldurur)
        var taskItemDto = _mapper.Map<TaskItemDetailDto>(taskItem);

        return Result<TaskItemDetailDto>.Success(taskItemDto);
    }
}
