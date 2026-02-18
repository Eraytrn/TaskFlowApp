using Microsoft.EntityFrameworkCore.Storage;
using TaskFlow.Domain.Interfaces;
using TaskFlow.Persistence.Repositories;

namespace TaskFlow.Persistence;

// Unit of Work Pattern
// Tüm repository'leri bir araya toplar
// Transaction yönetimi sağlar
// SaveChanges'i merkezi hale getirir
public class UnitOfWork : IUnitOfWork
{
    private readonly TaskFlowDbContext _context;
    private IDbContextTransaction? _transaction;

    // Lazy initialization - İlk kullanımda oluştur
    private IUserRepository? _users;
    private IBoardRepository? _boards;
    private ITaskItemRepository? _taskItems;
    private ICommentRepository? _comments;
    private IBoardMemberRepository? _boardMembers;
    private IBoardJoinRequestRepository? _joinRequests;
    private INotificationRepository? _notifications;
    private ILabelRepository? _labels;

    public UnitOfWork(TaskFlowDbContext context)
    {
        _context = context;
    }

    // Repository'lere erişim property'leri
    // İlk erişimde oluşturulur (Lazy Loading)
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IBoardRepository Boards => _boards ??= new BoardRepository(_context);
    public ITaskItemRepository TaskItems => _taskItems ??= new TaskItemRepository(_context);
    public ICommentRepository Comments => _comments ??= new CommentRepository(_context);
    public IBoardMemberRepository BoardMembers => _boardMembers ??= new BoardMemberRepository(_context);
    public IBoardJoinRequestRepository JoinRequests => _joinRequests ??= new BoardJoinRequestRepository(_context);
    public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
    public ILabelRepository Labels => _labels ??= new LabelRepository(_context);

    // Tüm değişiklikleri veritabanına kaydet
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    // Transaction başlat
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    // Transaction'ı onayla (Commit)
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction?.CommitAsync(cancellationToken)!;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    // Transaction'ı geri al (Rollback)
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _transaction?.RollbackAsync(cancellationToken)!;
        _transaction?.Dispose();
        _transaction = null;
    }

    // Dispose pattern - Kaynakları temizle
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}