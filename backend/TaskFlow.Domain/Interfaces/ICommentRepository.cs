using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetCommentsByTaskItemIdAsync(int taskItemId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
