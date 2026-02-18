using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Entities;

public class BoardMember : BaseEntity
{
    public int BoardId { get; private set; }
    public int UserId { get; private set; }
    public BoardMemberRole Role { get; private set; }
    public DateTime JoinedDate { get; private set; }

    // Navigation Properties
    public Board Board { get; private set; }
    public User User { get; private set; }

    private BoardMember() { }

    public BoardMember(int boardId, int userId, BoardMemberRole role)
    {
        BoardId = boardId;
        UserId = userId;
        Role = role;
        JoinedDate = DateTime.UtcNow;
    }

    public void ChangeRole(BoardMemberRole newRole)
    {
        Role = newRole;
    }
}
