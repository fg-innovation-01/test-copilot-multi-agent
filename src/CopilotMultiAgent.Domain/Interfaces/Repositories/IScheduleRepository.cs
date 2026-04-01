using CopilotMultiAgent.Domain.Entities;

namespace CopilotMultiAgent.Domain.Interfaces.Repositories;

public interface IScheduleRepository
{
    Task<IEnumerable<Schedule>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Schedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Schedule schedule, CancellationToken cancellationToken = default);
    Task UpdateAsync(Schedule schedule, CancellationToken cancellationToken = default);
    Task DeleteAsync(Schedule schedule, CancellationToken cancellationToken = default);
}
