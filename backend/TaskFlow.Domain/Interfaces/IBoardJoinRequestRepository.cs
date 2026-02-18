using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Interfaces;

public interface IBoardJoinRequestRepository : IRepository<BoardJoinRequest>
{
    Task<IEnumerable<BoardJoinRequest>> GetPendingRequestsByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoardJoinRequest>> GetRequestsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<BoardJoinRequest?> GetPendingRequestAsync(int boardId, int userId, CancellationToken cancellationToken = default);
    Task<bool> HasPendingRequestAsync(int boardId, int userId, CancellationToken cancellationToken = default);
}
