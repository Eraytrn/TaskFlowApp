using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence.Repositories;

// User entity'sine özel repository
// Email bazlı özel operasyonlar içerir
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TaskFlowDbContext context) : base(context)
    {
    }

    // Email'in sistemde kayıtlı olup olmadığını kontrol et
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken); 
    }

    // Email'e göre kullanıcı getir
    //FirstOrDefaultAsync : Bellekte olup olmadığına bakmaksızın veritabanına SELECT TOP(1) sorgusu gönderir.
    //Sadece ID ile değil, herhangi bir koşula (Lambda Expression) göre arama yapabilir.
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}

