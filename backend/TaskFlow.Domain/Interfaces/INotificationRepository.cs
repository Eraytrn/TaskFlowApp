using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetAllByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
}
