using System.ComponentModel.DataAnnotations;
using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.DTOs;

public record UpdateScheduleDto(
    [Required][MaxLength(200)] string Title,
    DateTime ScheduledAt,
    RecurrenceType RecurrenceType,
    bool IsCompleted);
