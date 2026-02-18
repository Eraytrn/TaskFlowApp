using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;


namespace TaskFlow.Persistence.Repositories;

public class BoardMemberRepository : Repository<BoardMember>, IBoardMemberRepository
{
    public BoardMemberRepository(TaskFlowDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BoardMember>> GetMembersByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardMembers
            .Include(bm => bm.User)
            .Where(bm => bm.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BoardMember>> GetBoardsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardMembers
            .Include(bm => bm.Board)
            .Where(bm => bm.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsMemberAsync(int boardId, int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardMembers
            .AnyAsync(bm => bm.BoardId == boardId && bm.UserId == userId, cancellationToken);
    }

    public async Task<bool> IsOwnerAsync(int boardId, int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardMembers
            .AnyAsync(bm => bm.BoardId == boardId && bm.UserId == userId && bm.Role == BoardMemberRole.Owner, cancellationToken);
    }

    public async Task<BoardMember?> GetMembershipAsync(int boardId, int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardMembers
            .FirstOrDefaultAsync(bm => bm.BoardId == boardId && bm.UserId == userId, cancellationToken);
    }
}
