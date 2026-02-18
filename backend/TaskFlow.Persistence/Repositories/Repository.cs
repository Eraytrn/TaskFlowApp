using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Common;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence.Repositories
{
    // Generic Repository - Tüm repository'lerin base sınıfı
    // CRUD operasyonlarının ortak implementasyonunu içerir
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly TaskFlowDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(TaskFlowDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // Yeni entity ekle (memory'ye, DB'ye SaveChanges ile yazılır)
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        // Birden fazla entity ekle
        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        // Koşula uyan kayıt var mı kontrol et
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        // Koşula göre entityleri bul (Where sorgusu)
        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        // Koşula göre ilk entity'yi getir veya null
        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public  virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        // Entity'yi sil
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        // Birden fazla entity sil
        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        // Entity'yi güncelle (ChangeTracker'a işaretle)
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        // Birden fazla entity güncelle
        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await Task.CompletedTask;
        }
    }
}
