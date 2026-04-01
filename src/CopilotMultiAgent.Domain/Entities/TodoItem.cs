using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Exceptions;

namespace CopilotMultiAgent.Domain.Entities;

public class TodoItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DueDate { get; private set; }
    public Priority Priority { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public List<string> Tags { get; private set; } = [];

    private TodoItem() { }

    public static TodoItem Create(
        string title,
        string? description = null,
        DateTime? dueDate = null,
        Priority priority = Priority.Low,
        IEnumerable<string>? tags = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty.");

        return new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            DueDate = dueDate,
            Priority = priority,
            Tags = tags?.ToList() ?? []
        };
    }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new DomainException("Title cannot be empty.");

        Title = newTitle.Trim();
        Touch();
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Todo item is already completed.");

        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }

    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        Touch();
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        Touch();
    }

    public void SetPriority(Priority priority)
    {
        Priority = priority;
        Touch();
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            throw new DomainException("Tag cannot be empty.");

        if (!Tags.Contains(tag))
            Tags.Add(tag);

        Touch();
    }

    public void RemoveTag(string tag)
    {
        Tags.Remove(tag);
        Touch();
    }

    public void SetTags(IEnumerable<string> tags)
    {
        Tags = tags.ToList();
        Touch();
    }

    public void SoftDelete()
    {
        if (IsDeleted)
            throw new DomainException("Todo item is already deleted.");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (!IsDeleted)
            throw new DomainException("Todo item is not deleted.");

        IsDeleted = false;
        DeletedAt = null;
    }

    private void Touch() => UpdatedAt = DateTime.UtcNow;
}
