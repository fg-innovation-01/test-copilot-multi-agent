using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Exceptions;

namespace CopilotMultiAgent.Domain.Entities;

public class Schedule
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime ScheduledAt { get; private set; }
    public RecurrenceType RecurrenceType { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private Schedule() { }

    public static Schedule Create(
        string title,
        DateTime scheduledAt,
        RecurrenceType recurrenceType = RecurrenceType.None)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty.");

        return new Schedule
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            ScheduledAt = scheduledAt,
            RecurrenceType = recurrenceType,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new DomainException("Title cannot be empty.");

        Title = newTitle.Trim();
    }

    public void UpdateScheduledAt(DateTime scheduledAt)
    {
        ScheduledAt = scheduledAt;
    }

    public void UpdateRecurrence(RecurrenceType recurrenceType)
    {
        RecurrenceType = recurrenceType;
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Schedule is already completed.");

        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
    }

    public void SoftDelete()
    {
        if (IsDeleted)
            throw new DomainException("Schedule is already deleted.");

        IsDeleted = true;
    }
}
