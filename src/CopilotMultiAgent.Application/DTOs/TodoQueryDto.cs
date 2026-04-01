using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.DTOs;

public class TodoQueryDto
{
    public bool? IsCompleted { get; init; }
    public Priority? Priority { get; init; }
    public string? Tag { get; init; }
    public string? SortBy { get; init; }
    public string? SortDirection { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
