namespace TaskFlow.Application.Features.Users.DTOs;

/// <summary>
/// User entity'sinin API response versiyonu
/// Sensitive data yok (PasswordHash gizli)
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
