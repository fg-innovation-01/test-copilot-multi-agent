using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Exceptions;
using FluentAssertions;

namespace CopilotMultiAgent.Domain.Tests.Entities;

public class TodoItemTests
{
    [Fact]
    public void Create_WithValidTitle_ShouldCreateTodoItem()
    {
        // Arrange & Act
        var todo = TodoItem.Create("Buy groceries");

        // Assert
        todo.Id.Should().NotBeEmpty();
        todo.Title.Should().Be("Buy groceries");
        todo.IsCompleted.Should().BeFalse();
        todo.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyTitle_ShouldThrowDomainException(string invalidTitle)
    {
        // Act
        var act = () => TodoItem.Create(invalidTitle);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Title cannot be empty.");
    }

    [Fact]
    public void Create_WithWhitespacePaddedTitle_ShouldTrimTitle()
    {
        var todo = TodoItem.Create("  Buy groceries  ");
        todo.Title.Should().Be("Buy groceries");
    }

    [Fact]
    public void Complete_WhenNotCompleted_ShouldSetIsCompletedTrue()
    {
        var todo = TodoItem.Create("Task");
        todo.Complete();
        todo.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_ShouldThrowDomainException()
    {
        var todo = TodoItem.Create("Task");
        todo.Complete();

        var act = () => todo.Complete();
        act.Should().Throw<DomainException>()
            .WithMessage("Todo item is already completed.");
    }

    [Fact]
    public void Reopen_WhenCompleted_ShouldSetIsCompletedFalse()
    {
        var todo = TodoItem.Create("Task");
        todo.Complete();
        todo.Reopen();
        todo.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void UpdateTitle_WithValidTitle_ShouldChangeTitle()
    {
        var todo = TodoItem.Create("Old title");
        todo.UpdateTitle("New title");
        todo.Title.Should().Be("New title");
    }

    [Fact]
    public void UpdateTitle_WithEmptyTitle_ShouldThrowDomainException()
    {
        var todo = TodoItem.Create("Task");
        var act = () => todo.UpdateTitle("");
        act.Should().Throw<DomainException>();
    }
}
