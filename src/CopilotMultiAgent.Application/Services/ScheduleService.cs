using CopilotMultiAgent.Application.Common.Exceptions;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Interfaces.Repositories;

namespace CopilotMultiAgent.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;

    public ScheduleService(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ScheduleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return items.Select(MapToDto);
    }

    public async Task<ScheduleDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var schedule = await GetScheduleOrThrowAsync(id, cancellationToken);
        return MapToDto(schedule);
    }

    public async Task<ScheduleDto> CreateAsync(CreateScheduleDto dto, CancellationToken cancellationToken = default)
    {
        var schedule = Schedule.Create(dto.Title, dto.ScheduledAt, dto.RecurrenceType);
        await _repository.AddAsync(schedule, cancellationToken);
        return MapToDto(schedule);
    }

    public async Task<ScheduleDto> UpdateAsync(Guid id, UpdateScheduleDto dto, CancellationToken cancellationToken = default)
    {
        var schedule = await GetScheduleOrThrowAsync(id, cancellationToken);

        schedule.UpdateTitle(dto.Title);
        schedule.UpdateScheduledAt(dto.ScheduledAt);
        schedule.UpdateRecurrence(dto.RecurrenceType);

        if (dto.IsCompleted && !schedule.IsCompleted)
            schedule.Complete();
        else if (!dto.IsCompleted && schedule.IsCompleted)
            schedule.Reopen();

        await _repository.UpdateAsync(schedule, cancellationToken);
        return MapToDto(schedule);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var schedule = await GetScheduleOrThrowAsync(id, cancellationToken);
        schedule.SoftDelete();
        await _repository.UpdateAsync(schedule, cancellationToken);
    }

    public async Task<ScheduleDto> CompleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var schedule = await GetScheduleOrThrowAsync(id, cancellationToken);
        schedule.Complete();
        await _repository.UpdateAsync(schedule, cancellationToken);
        return MapToDto(schedule);
    }

    private async Task<Schedule> GetScheduleOrThrowAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Schedule), id);
    }

    private static ScheduleDto MapToDto(Schedule schedule)
    {
        return new ScheduleDto(
            schedule.Id,
            schedule.Title,
            schedule.ScheduledAt,
            schedule.RecurrenceType,
            schedule.IsCompleted,
            schedule.CreatedAt,
            schedule.IsDeleted);
    }
}
