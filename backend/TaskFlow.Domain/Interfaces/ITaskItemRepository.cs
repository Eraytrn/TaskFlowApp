using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface ITaskItemRepository : IRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetTasksByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItem>> GetTasksByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<TaskItem?> GetTaskWithCommentsAsync(int taskId, CancellationToken cancellationToken = default);
}
