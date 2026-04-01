using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Interfaces.Repositories;
using CopilotMultiAgent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CopilotMultiAgent.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly AppDbContext _context;

    public ScheduleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Schedule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Schedules.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task AddAsync(Schedule schedule, CancellationToken cancellationToken = default)
    {
        await _context.Schedules.AddAsync(schedule, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Schedule schedule, CancellationToken cancellationToken = default)
    {
        _context.Schedules.Update(schedule);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Schedule schedule, CancellationToken cancellationToken = default)
    {
        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
