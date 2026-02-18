using AutoMapper;
using TaskFlow.Application.Features.Users.DTOs;
using TaskFlow.Application.Features.Boards.DTOs;
using TaskFlow.Application.Features.TaskItems.DTOs;
using TaskFlow.Application.Features.Comments.DTOs;
using TaskFlow.Application.DTOs;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Common.Mappings;

/// <summary>
/// AutoMapper profili - Entity <-> DTO otomatik dönüşümü
/// ForMember ile navigation property'lerden değer alma
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();

        // TaskItem mappings
        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(dest => dest.BoardName,
                opt => opt.MapFrom(src => src.Board != null ? src.Board.Title : "Unknown"))
            .ForMember(dest => dest.AssignedUserName,
                opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : null))
            .ForMember(dest => dest.StatusText,
                opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<TaskItem, TaskItemDetailDto>()
            .ForMember(dest => dest.BoardName,
                opt => opt.MapFrom(src => src.Board != null ? src.Board.Title : "Unknown"))
            .ForMember(dest => dest.AssignedUserName,
                opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : null))
            .ForMember(dest => dest.StatusText,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Comments,
                opt => opt.MapFrom(src => src.Comments));

        // Comment mappings
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FullName : "Unknown"));

        // Board mappings
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner != null ? src.Owner.FullName : "Unknown"));

        CreateMap<Board, BoardDetailDto>()
            .ForMember(dest => dest.OwnerName,
                opt => opt.MapFrom(src => src.Owner != null ? src.Owner.FullName : "Unknown"))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.TotalTaskCount, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedTaskCount, opt => opt.Ignore());

        // BoardMember mappings
        CreateMap<BoardMember, BoardMemberDto>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User != null ? src.User.FullName : "Unknown"))
            .ForMember(dest => dest.UserEmail,
                opt => opt.MapFrom(src => src.User != null ? src.User.Email : ""));

        // BoardJoinRequest mappings
        CreateMap<BoardJoinRequest, BoardJoinRequestDto>()
            .ForMember(dest => dest.BoardTitle,
                opt => opt.MapFrom(src => src.Board != null ? src.Board.Title : "Unknown"))
            .ForMember(dest => dest.RequesterName,
                opt => opt.MapFrom(src => src.Requester != null ? src.Requester.FullName : "Unknown"))
            .ForMember(dest => dest.RequesterEmail,
                opt => opt.MapFrom(src => src.Requester != null ? src.Requester.Email : ""))
            .ForMember(dest => dest.ResponderName,
                opt => opt.MapFrom(src => src.Responder != null ? src.Responder.FullName : null));

        // Notification mappings
        CreateMap<Notification, NotificationDto>();

        // Label mappings
        CreateMap<Label, TaskFlow.Application.Features.Labels.DTOs.LabelDto>();
    }
}
