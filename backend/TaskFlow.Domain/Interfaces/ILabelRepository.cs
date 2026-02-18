using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Interfaces;

public interface ILabelRepository : IRepository<Label>
{
    Task<IEnumerable<Label>> GetByBoardIdAsync(int boardId, CancellationToken cancellationToken = default);
}
