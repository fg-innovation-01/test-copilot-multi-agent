using CopilotMultiAgent.Domain.Entities;

namespace CopilotMultiAgent.Domain.Interfaces.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TodoItem?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TodoItem todo, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoItem todo, CancellationToken cancellationToken = default);
    Task DeleteAsync(TodoItem todo, CancellationToken cancellationToken = default);
}
