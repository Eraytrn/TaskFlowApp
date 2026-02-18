using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface IBoardMemberRepository : IRepository<BoardMember>
{
    Task<IEnumerable<BoardMember>> GetMembersByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardMember>> GetBoardsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> IsMemberAsync(int boardId, int userId, CancellationToken cancellationToken = default);
    Task<bool> IsOwnerAsync(int boardId, int userId, CancellationToken cancellationToken = default);
    Task<BoardMember?> GetMembershipAsync(int boardId, int userId, CancellationToken cancellationToken = default);
}
