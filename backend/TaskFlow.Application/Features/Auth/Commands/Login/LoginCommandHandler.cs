using MediatR;
using TaskFlow.Application.Common.Interfaces;
using TaskFlow.Application.Common.Models;
using TaskFlow.Application.Features.Auth.DTOs;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Email ile kullanıcıyı bul
        var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
        var user = users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null)
        {
            return Result<AuthResponseDto>.Failure("Geçersiz email veya şifre.");
        }

        // Şifreyi kontrol et
        if (!user.VerifyPassword(request.Password))
        {
            return Result<AuthResponseDto>.Failure("Geçersiz email veya şifre.");
        }

        // JWT token oluştur
        var token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.FullName);

        var response = new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60) // appsettings'den oku daha iyi ama şimdilik sabit
        };

        return Result<AuthResponseDto>.Success(response);
    }
}
