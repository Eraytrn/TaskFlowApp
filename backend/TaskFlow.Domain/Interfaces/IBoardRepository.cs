using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface IBoardRepository : IRepository<Board>
{
    Task<IEnumerable<Board>> GetBoardsByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default);
    Task<Board?> GetBoardWithTasksAsync(int boardId, CancellationToken cancellationToken = default);
}
