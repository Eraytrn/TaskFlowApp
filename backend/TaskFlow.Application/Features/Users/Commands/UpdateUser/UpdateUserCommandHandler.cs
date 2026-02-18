using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Users.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // User'ı bul
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            return Result<UserDto>.Failure("Kullanıcı bulunamadı.");
        }

        // Email değişmişse ve başka birinde varsa hata
        if (user.Email != request.Email)
        {
            var emailExists = await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken);
            if (emailExists)
            {
                return Result<UserDto>.Failure("Bu email adresi zaten kullanılıyor.");
            }
        }

        // User'ı güncelle (Domain'de Update metodu yok, direkt property set ediyoruz)
        // İleride User entity'sine UpdateProfile metodu ekleyebiliriz
        user.GetType().GetProperty("FullName")?.SetValue(user, request.FullName);
        user.GetType().GetProperty("Email")?.SetValue(user, request.Email);

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var userDto = _mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto);
    }
}
