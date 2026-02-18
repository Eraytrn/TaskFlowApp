using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence.Repositories;

// Comment entity'sine özel repository
public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(TaskFlowDbContext context) : base(context)
    {
    }

    // Bir task'a ait tüm yorumları getir
    public async Task<IEnumerable<Comment>> GetCommentsByTaskItemIdAsync(int taskItemId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.User) // Yorum yapan kullanıcı bilgisi
            .Where(c => c.TaskItemId == taskItemId)
            .OrderBy(c => c.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    // Kullanıcının yaptığı tüm yorumları getir
    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.TaskItem) // Hangi task'a yorum yaptı
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync(cancellationToken);
    }
}