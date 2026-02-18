using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence.Repositories
{
    public class BoardRepository : Repository<Board>, IBoardRepository
    {
        public BoardRepository(TaskFlowDbContext context) : base(context) 
        { 
        }


        // Kullanıcının sahip olduğu tüm boardları getir (Owner bilgisiyle)
        public async Task<IEnumerable<Board>> GetBoardsByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(b => b.Owner)
                .Where(b => b.OwnerId == ownerId)
                .OrderByDescending(b => b.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        // Board'u task'ları ve owner ile birlikte getir (Eager Loading)
        // Include ile ilişkili verileri de çeker
        public async Task<Board?> GetBoardWithTasksAsync(int boardId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(b => b.Owner)
                .Include(b => b.Tasks)
                    .ThenInclude(t => t.Labels)
                .Include(b => b.Labels)
                .Include(b => b.Members) // For access control
                .FirstOrDefaultAsync(b => b.Id == boardId, cancellationToken);
        }
    }
}
