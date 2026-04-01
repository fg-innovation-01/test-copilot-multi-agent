using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ScheduleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ScheduleDto> CreateAsync(CreateScheduleDto dto, CancellationToken cancellationToken = default);
    Task<ScheduleDto> UpdateAsync(Guid id, UpdateScheduleDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ScheduleDto> CompleteAsync(Guid id, CancellationToken cancellationToken = default);
}
