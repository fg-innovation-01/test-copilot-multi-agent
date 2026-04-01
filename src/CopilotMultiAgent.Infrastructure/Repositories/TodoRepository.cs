using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Interfaces.Repositories;
using CopilotMultiAgent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CopilotMultiAgent.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TodoItems.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<TodoItem?> GetByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.TodoItems
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task AddAsync(TodoItem todo, CancellationToken cancellationToken = default)
    {
        await _context.TodoItems.AddAsync(todo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TodoItem todo, CancellationToken cancellationToken = default)
    {
        _context.TodoItems.Update(todo);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TodoItem todo, CancellationToken cancellationToken = default)
    {
        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
