using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;


namespace TaskFlow.Persistence.Repositories;

public class BoardJoinRequestRepository : Repository<BoardJoinRequest>, IBoardJoinRequestRepository
{
    public BoardJoinRequestRepository(TaskFlowDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BoardJoinRequest>> GetPendingRequestsByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardJoinRequests
            .Include(r => r.Requester)
            .Include(r => r.Board)
            .Where(r => r.BoardId == boardId && r.Status == RequestStatus.Pending)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BoardJoinRequest>> GetRequestsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardJoinRequests
            .Include(r => r.Board)
            .Where(r => r.RequesterId == userId)
            .OrderByDescending(r => r.RequestDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<BoardJoinRequest?> GetPendingRequestAsync(int boardId, int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardJoinRequests
            .FirstOrDefaultAsync(r => r.BoardId == boardId && r.RequesterId == userId && r.Status == RequestStatus.Pending, cancellationToken);
    }

    public async Task<bool> HasPendingRequestAsync(int boardId, int userId, CancellationToken cancellationToken = default)
    {
        return await _context.BoardJoinRequests
            .AnyAsync(r => r.BoardId == boardId && r.RequesterId == userId && r.Status == RequestStatus.Pending, cancellationToken);
    }

    // Override GetByIdAsync to include navigation properties
    public override async Task<BoardJoinRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.BoardJoinRequests
            .Include(r => r.Board)
            .Include(r => r.Requester)
            .Include(r => r.Responder)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
