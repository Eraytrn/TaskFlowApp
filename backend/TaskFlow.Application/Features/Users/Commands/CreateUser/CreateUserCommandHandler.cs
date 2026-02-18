using AutoMapper;
using MediatR;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Users.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Users.Commands.CreateUser;

/// <summary>
/// CreateUserCommand'i işleyen handler
/// IRequestHandler<TRequest, TResponse> implement eder
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Email zaten var mı kontrol et
        var emailExists = await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken);
        if (emailExists)
        {
            return Result<UserDto>.Failure("Bu email adresi zaten kayıtlı.");
        }

        // Şifre hash'le (şimdilik basit, ileride BCrypt kullanacağız)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // User entity oluştur
        var user = new User(request.FullName, request.Email, passwordHash);

        // Repository'ye ekle
        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        
        // Veritabanına kaydet
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Entity'yi DTO'ya dönüştür
        var userDto = _mapper.Map<UserDto>(user);

        return Result<UserDto>.Success(userDto);
    }
}
