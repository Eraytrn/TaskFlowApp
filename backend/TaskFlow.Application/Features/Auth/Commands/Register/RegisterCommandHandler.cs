using MediatR;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Auth.DTOs;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Email kontrolü
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
        var existingUser = users.FirstOrDefault(u => u.Email == request.Email);

        if (existingUser != null)
        {
            return Result<AuthResponseDto>.Failure("Bu email adresi zaten kayıtlı.");
        }

        // Yeni kullanıcı oluştur
        var user = new User(request.FullName, request.Email, request.Password);

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // JWT token oluştur
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.FullName);

        var response = new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };

        return Result<AuthResponseDto>.Success(response);
    }
}
