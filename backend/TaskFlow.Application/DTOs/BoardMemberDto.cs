using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class BoardMemberDto
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public BoardMemberRole Role { get; set; }
    public DateTime JoinedDate { get; set; }
}
