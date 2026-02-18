using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence.Repositories;

public class TaskItemRepository : Repository<TaskItem>, ITaskItemRepository
{
    public TaskItemRepository(TaskFlowDbContext context) : base(context)
    {
    }

    // Kullanıcıya atanmış task'ları getir (Board ve User bilgileriyle)
    public async Task<IEnumerable<TaskItem>> GetTasksByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Board)
            .Include(t => t.Board)
            .Include(t => t.AssignedUser)
            .Include(t => t.Labels)
            .Where(t => t.AssignedUserId == userId)
            .OrderBy(t => t.Status)
            .ToListAsync(cancellationToken);
    }

    // Bir board'a ait tüm task'ları getir (Board ve User bilgileriyle)
    public async Task<IEnumerable<TaskItem>> GetTasksByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Board)
            .Include(t => t.Board)
            .Include(t => t.AssignedUser)
            .Include(t => t.Labels)
            .Where(t => t.BoardId == boardId)
            .OrderBy(t => t.Status)
            .ThenByDescending(t => t.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    // Task'ı commentleri ile birlikte getir (Detay sayfası için)
    public async Task<TaskItem?> GetTaskWithCommentsAsync(int taskId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Board)
            .Include(t => t.Board)
            .Include(t => t.AssignedUser)
            .Include(t => t.Labels)
            .Include(t => t.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
    }
}
