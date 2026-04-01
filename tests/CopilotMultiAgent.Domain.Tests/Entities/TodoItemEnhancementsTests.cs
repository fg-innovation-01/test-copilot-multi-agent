using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Exceptions;
using FluentAssertions;

namespace CopilotMultiAgent.Domain.Tests.Entities;

public class TodoItemEnhancementsTests
{
    // -------------------------------------------------------------------------
    // Priority
    // -------------------------------------------------------------------------

    [Fact]
    public void Create_WithDefaultArgs_ShouldHavePriorityLow()
    {
        var todo = TodoItem.Create("Task");

        todo.Priority.Should().Be(Priority.Low);
    }

    [Fact]
    public void Create_WithExplicitPriority_ShouldSetPriority()
    {
        var todo = TodoItem.Create("Task", priority: Priority.High);

        todo.Priority.Should().Be(Priority.High);
    }

    [Fact]
    public void SetPriority_ShouldUpdatePriority()
    {
        var todo = TodoItem.Create("Task");

        todo.SetPriority(Priority.Medium);

        todo.Priority.Should().Be(Priority.Medium);
    }

    [Fact]
    public void SetPriority_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.SetPriority(Priority.High);

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    // -------------------------------------------------------------------------
    // Description
    // -------------------------------------------------------------------------

    [Fact]
    public void Create_WithDescription_ShouldSetDescription()
    {
        var todo = TodoItem.Create("Task", description: "Detailed description");

        todo.Description.Should().Be("Detailed description");
    }

    [Fact]
    public void Create_WithNoDescription_ShouldHaveNullDescription()
    {
        var todo = TodoItem.Create("Task");

        todo.Description.Should().BeNull();
    }

    [Fact]
    public void UpdateDescription_WithValue_ShouldSetDescription()
    {
        var todo = TodoItem.Create("Task");

        todo.UpdateDescription("New description");

        todo.Description.Should().Be("New description");
    }

    [Fact]
    public void UpdateDescription_WithValue_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.UpdateDescription("New description");

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdateDescription_WithNull_ShouldClearDescription()
    {
        var todo = TodoItem.Create("Task", description: "Initial");

        todo.UpdateDescription(null);

        todo.Description.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // DueDate
    // -------------------------------------------------------------------------

    [Fact]
    public void Create_WithDueDate_ShouldSetDueDate()
    {
        var due = DateTime.UtcNow.AddDays(7);

        var todo = TodoItem.Create("Task", dueDate: due);

        todo.DueDate.Should().BeCloseTo(due, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithNoDueDate_ShouldHaveNullDueDate()
    {
        var todo = TodoItem.Create("Task");

        todo.DueDate.Should().BeNull();
    }

    [Fact]
    public void SetDueDate_WithValue_ShouldSetDueDate()
    {
        var todo = TodoItem.Create("Task");
        var due = DateTime.UtcNow.AddDays(3);

        todo.SetDueDate(due);

        todo.DueDate.Should().BeCloseTo(due, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SetDueDate_WithValue_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.SetDueDate(DateTime.UtcNow.AddDays(3));

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SetDueDate_WithNull_ShouldClearDueDate()
    {
        var todo = TodoItem.Create("Task", dueDate: DateTime.UtcNow.AddDays(3));

        todo.SetDueDate(null);

        todo.DueDate.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // Audit fields: UpdatedAt, CompletedAt
    // -------------------------------------------------------------------------

    [Fact]
    public void Create_ShouldHaveNullUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void UpdateTitle_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.UpdateTitle("New title");

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Complete_ShouldSetCompletedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.Complete();

        todo.CompletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Reopen_ShouldClearCompletedAt()
    {
        var todo = TodoItem.Create("Task");
        todo.Complete();

        todo.Reopen();

        todo.CompletedAt.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // Tags
    // -------------------------------------------------------------------------

    [Fact]
    public void Create_WithTags_ShouldSetTags()
    {
        var todo = TodoItem.Create("Task", tags: ["work", "urgent"]);

        todo.Tags.Should().BeEquivalentTo(["work", "urgent"]);
    }

    [Fact]
    public void Create_WithNoTags_ShouldHaveEmptyTagsList()
    {
        var todo = TodoItem.Create("Task");

        todo.Tags.Should().BeEmpty();
    }

    [Fact]
    public void AddTag_ShouldAddTagToList()
    {
        var todo = TodoItem.Create("Task");

        todo.AddTag("work");

        todo.Tags.Should().Contain("work");
    }

    [Fact]
    public void AddTag_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.AddTag("work");

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void AddTag_WithDuplicate_ShouldNotAddTwice()
    {
        var todo = TodoItem.Create("Task");
        todo.AddTag("work");

        todo.AddTag("work");

        todo.Tags.Should().HaveCount(1);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AddTag_WithEmptyOrWhitespace_ShouldThrowDomainException(string invalidTag)
    {
        var todo = TodoItem.Create("Task");

        var act = () => todo.AddTag(invalidTag);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void RemoveTag_ShouldRemoveTagFromList()
    {
        var todo = TodoItem.Create("Task", tags: ["work", "personal"]);

        todo.RemoveTag("work");

        todo.Tags.Should().NotContain("work");
        todo.Tags.Should().Contain("personal");
    }

    [Fact]
    public void RemoveTag_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task", tags: ["work"]);

        todo.RemoveTag("work");

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void RemoveTag_WhenTagNotPresent_ShouldNotThrow()
    {
        var todo = TodoItem.Create("Task");

        var act = () => todo.RemoveTag("nonexistent");

        act.Should().NotThrow();
    }

    [Fact]
    public void SetTags_ShouldReplaceAllExistingTags()
    {
        var todo = TodoItem.Create("Task", tags: ["old-tag"]);

        todo.SetTags(["new1", "new2"]);

        todo.Tags.Should().BeEquivalentTo(["new1", "new2"]);
        todo.Tags.Should().NotContain("old-tag");
    }

    [Fact]
    public void SetTags_ShouldSetUpdatedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.SetTags(["tag1"]);

        todo.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SetTags_WithEmptyCollection_ShouldClearAllTags()
    {
        var todo = TodoItem.Create("Task", tags: ["work"]);

        todo.SetTags([]);

        todo.Tags.Should().BeEmpty();
    }

    // -------------------------------------------------------------------------
    // Soft Delete
    // -------------------------------------------------------------------------

    [Fact]
    public void Create_ShouldHaveIsDeletedFalse()
    {
        var todo = TodoItem.Create("Task");

        todo.IsDeleted.Should().BeFalse();
        todo.DeletedAt.Should().BeNull();
    }

    [Fact]
    public void SoftDelete_ShouldSetIsDeletedTrue()
    {
        var todo = TodoItem.Create("Task");

        todo.SoftDelete();

        todo.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void SoftDelete_ShouldSetDeletedAt()
    {
        var todo = TodoItem.Create("Task");

        todo.SoftDelete();

        todo.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void SoftDelete_WhenAlreadyDeleted_ShouldThrowDomainException()
    {
        var todo = TodoItem.Create("Task");
        todo.SoftDelete();

        var act = () => todo.SoftDelete();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Restore_AfterSoftDelete_ShouldSetIsDeletedFalse()
    {
        var todo = TodoItem.Create("Task");
        todo.SoftDelete();

        todo.Restore();

        todo.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Restore_AfterSoftDelete_ShouldClearDeletedAt()
    {
        var todo = TodoItem.Create("Task");
        todo.SoftDelete();

        todo.Restore();

        todo.DeletedAt.Should().BeNull();
    }

    [Fact]
    public void Restore_WhenNotDeleted_ShouldThrowDomainException()
    {
        var todo = TodoItem.Create("Task");

        var act = () => todo.Restore();

        act.Should().Throw<DomainException>();
    }
}
