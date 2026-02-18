using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Interfaces;

namespace TaskFlow.Persistence.Repositories;

public class LabelRepository : Repository<Label>, ILabelRepository
{
    public LabelRepository(TaskFlowDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default)
    {
        return await _context.Labels
            .Where(l => l.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }
}
