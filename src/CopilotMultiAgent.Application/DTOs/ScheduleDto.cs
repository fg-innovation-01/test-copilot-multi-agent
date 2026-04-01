using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.DTOs;

public record ScheduleDto(
    Guid Id,
    string Title,
    DateTime ScheduledAt,
    RecurrenceType RecurrenceType,
    bool IsCompleted,
    DateTime CreatedAt,
    bool IsDeleted);
