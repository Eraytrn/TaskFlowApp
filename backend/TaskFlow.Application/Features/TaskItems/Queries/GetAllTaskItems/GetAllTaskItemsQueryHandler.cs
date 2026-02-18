using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.TaskItems.Queries.GetAllTaskItems;

public class GetAllTaskItemsQueryHandler : IRequestHandler<GetAllTaskItemsQuery, Result<PaginatedList<TaskItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTaskItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<TaskItemDto>>> Handle(GetAllTaskItemsQuery request, CancellationToken cancellationToken)
    {
        // Tüm task'ları getir
        var taskItems = await _unitOfWork.TaskItems.GetAllAsync(cancellationToken);

        // DTO'ya çevir (Not: GetAllAsync Include yapmıyor, manuel yapmalıyız veya basit bilgiler için kabul edelim)
        var taskItemDtos = _mapper.Map<List<TaskItemDto>>(taskItems);

        // Paginate et
        var paginatedTasks = PaginatedList<TaskItemDto>.Create(
            taskItemDtos.AsQueryable(),
            request.PageNumber,
            request.PageSize
        );

        return Result<PaginatedList<TaskItemDto>>.Success(paginatedTasks);
    }
}
