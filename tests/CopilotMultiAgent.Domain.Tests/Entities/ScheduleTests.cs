using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Exceptions;
using FluentAssertions;

namespace CopilotMultiAgent.Domain.Tests.Entities;

public class ScheduleTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldCreateSchedule()
    {
        // Arrange
        var scheduledAt = DateTime.UtcNow.AddDays(1);

        // Act
        var schedule = Schedule.Create("Team meeting", scheduledAt);

        // Assert
        schedule.Id.Should().NotBeEmpty();
        schedule.Title.Should().Be("Team meeting");
        schedule.ScheduledAt.Should().Be(scheduledAt);
        schedule.RecurrenceType.Should().Be(RecurrenceType.None);
        schedule.IsCompleted.Should().BeFalse();
        schedule.IsDeleted.Should().BeFalse();
        schedule.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyTitle_ShouldThrowDomainException(string invalidTitle)
    {
        // Arrange
        var scheduledAt = DateTime.UtcNow.AddDays(1);

        // Act
        var act = () => Schedule.Create(invalidTitle, scheduledAt);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Title cannot be empty.");
    }

    [Fact]
    public void Create_WithWhitespacePaddedTitle_ShouldTrimTitle()
    {
        // Act
        var schedule = Schedule.Create("  Team meeting  ", DateTime.UtcNow.AddDays(1));

        // Assert
        schedule.Title.Should().Be("Team meeting");
    }

    [Fact]
    public void Complete_WhenNotCompleted_ShouldSetIsCompletedTrue()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));

        // Act
        schedule.Complete();

        // Assert
        schedule.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_ShouldThrowDomainException()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));
        schedule.Complete();

        // Act
        var act = () => schedule.Complete();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Schedule is already completed.");
    }

    [Fact]
    public void Reopen_WhenCompleted_ShouldSetIsCompletedFalse()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));
        schedule.Complete();

        // Act
        schedule.Reopen();

        // Assert
        schedule.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void UpdateTitle_WithEmptyTitle_ShouldThrowDomainException()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));

        // Act
        var act = () => schedule.UpdateTitle("");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Title cannot be empty.");
    }

    [Fact]
    public void SoftDelete_WhenNotDeleted_ShouldSetIsDeletedTrue()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));

        // Act
        schedule.SoftDelete();

        // Assert
        schedule.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void SoftDelete_WhenAlreadyDeleted_ShouldThrowDomainException()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));
        schedule.SoftDelete();

        // Act
        var act = () => schedule.SoftDelete();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Schedule is already deleted.");
    }

    [Theory]
    [InlineData(RecurrenceType.Daily)]
    [InlineData(RecurrenceType.Weekly)]
    [InlineData(RecurrenceType.Monthly)]
    [InlineData(RecurrenceType.Yearly)]
    public void Create_WithExplicitRecurrenceType_ShouldSetRecurrenceType(RecurrenceType recurrenceType)
    {
        // Act
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1), recurrenceType);

        // Assert
        schedule.RecurrenceType.Should().Be(recurrenceType);
    }

    [Fact]
    public void UpdateTitle_WithValidTitle_ShouldChangeTitle()
    {
        // Arrange
        var schedule = Schedule.Create("Old title", DateTime.UtcNow.AddDays(1));

        // Act
        schedule.UpdateTitle("New title");

        // Assert
        schedule.Title.Should().Be("New title");
    }

    [Fact]
    public void UpdateTitle_WithWhitespacePaddedTitle_ShouldTrimTitle()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1));

        // Act
        schedule.UpdateTitle("  Sprint review  ");

        // Assert
        schedule.Title.Should().Be("Sprint review");
    }

    [Fact]
    public void UpdateScheduledAt_ShouldUpdateScheduledAt()
    {
        // Arrange
        var original = DateTime.UtcNow.AddDays(1);
        var updated = DateTime.UtcNow.AddDays(10);
        var schedule = Schedule.Create("Task", original);

        // Act
        schedule.UpdateScheduledAt(updated);

        // Assert
        schedule.ScheduledAt.Should().Be(updated);
    }

    [Fact]
    public void UpdateRecurrence_ShouldUpdateRecurrenceType()
    {
        // Arrange
        var schedule = Schedule.Create("Task", DateTime.UtcNow.AddDays(1), RecurrenceType.None);

        // Act
        schedule.UpdateRecurrence(RecurrenceType.Weekly);

        // Assert
        schedule.RecurrenceType.Should().Be(RecurrenceType.Weekly);
    }
}
