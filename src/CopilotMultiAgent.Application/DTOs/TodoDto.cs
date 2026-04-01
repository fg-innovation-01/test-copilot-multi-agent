using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.DTOs;

public record TodoDto(
    Guid Id,
    string Title,
    bool IsCompleted,
    DateTime CreatedAt,
    string? Description,
    DateTime? DueDate,
    Priority Priority,
    DateTime? UpdatedAt,
    DateTime? CompletedAt,
    IReadOnlyList<string> Tags,
    bool IsDeleted);
