using CopilotMultiAgent.Application.Common.Exceptions;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Interfaces.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace CopilotMultiAgent.Application.Tests.Services;

public class TodoServiceEnhancementsTests
{
    private readonly ITodoRepository _repository = Substitute.For<ITodoRepository>();
    private readonly TodoService _sut;

    public TodoServiceEnhancementsTests()
    {
        _sut = new TodoService(_repository);
    }

    // -------------------------------------------------------------------------
    // GetAllAsync — returns PagedResultDto
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_WithDefaultQuery_ShouldReturnPagedResult()
    {
        // Arrange
        var items = new List<TodoItem>
        {
            TodoItem.Create("Task 1"),
            TodoItem.Create("Task 2"),
            TodoItem.Create("Task 3"),
        };
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto();

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Should().BeOfType<PagedResultDto<TodoDto>>();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetAllAsync_FilterByIsCompleted_ShouldReturnOnlyCompletedItems()
    {
        // Arrange
        var completed = TodoItem.Create("Done task");
        completed.Complete();
        var items = new List<TodoItem> { TodoItem.Create("Active task"), completed };
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto { IsCompleted = true };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().IsCompleted.Should().BeTrue();
        result.TotalCount.Should().Be(1);
    }

    [Fact]
    public async Task GetAllAsync_FilterByIsNotCompleted_ShouldReturnOnlyActiveItems()
    {
        // Arrange
        var completed = TodoItem.Create("Done task");
        completed.Complete();
        var items = new List<TodoItem> { TodoItem.Create("Active task"), completed };
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto { IsCompleted = false };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_FilterByPriority_ShouldReturnOnlyMatchingItems()
    {
        // Arrange
        var highTask = TodoItem.Create("High priority");
        highTask.SetPriority(Priority.High);
        var items = new List<TodoItem> { TodoItem.Create("Low task"), highTask };
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto { Priority = Priority.High };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Priority.Should().Be(Priority.High);
    }

    [Fact]
    public async Task GetAllAsync_FilterByTag_ShouldReturnOnlyItemsWithThatTag()
    {
        // Arrange
        var workTask = TodoItem.Create("Work task", tags: ["work"]);
        var personalTask = TodoItem.Create("Personal task", tags: ["personal"]);
        var items = new List<TodoItem> { workTask, personalTask, TodoItem.Create("No tag") };
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto { Tag = "work" };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().Tags.Should().Contain("work");
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var items = Enumerable.Range(1, 5).Select(i => TodoItem.Create($"Task {i}")).ToList();
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto { Page = 2, PageSize = 2 };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(5);
        result.Page.Should().Be(2);
        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task GetAllAsync_WithPaginationOnLastPage_ShouldReturnRemainingItems()
    {
        // Arrange
        var items = Enumerable.Range(1, 5).Select(i => TodoItem.Create($"Task {i}")).ToList();
        _repository.GetAllAsync().Returns(items);
        var query = new TodoQueryDto { Page = 3, PageSize = 2 };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.Should().HaveCount(1);
        result.TotalCount.Should().Be(5);
    }

    [Fact]
    public async Task GetAllAsync_SortByDueDateAscending_ShouldReturnItemsInOrder()
    {
        // Arrange
        var earlyTask = TodoItem.Create("Early");
        earlyTask.SetDueDate(DateTime.UtcNow.AddDays(1));
        var lateTask = TodoItem.Create("Late");
        lateTask.SetDueDate(DateTime.UtcNow.AddDays(10));
        _repository.GetAllAsync().Returns([lateTask, earlyTask]); // Intentionally reversed
        var query = new TodoQueryDto { SortBy = "duedate", SortDirection = "asc" };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.First().Title.Should().Be("Early");
    }

    [Fact]
    public async Task GetAllAsync_SortByDueDateDescending_ShouldReturnItemsInOrder()
    {
        // Arrange
        var earlyTask = TodoItem.Create("Early");
        earlyTask.SetDueDate(DateTime.UtcNow.AddDays(1));
        var lateTask = TodoItem.Create("Late");
        lateTask.SetDueDate(DateTime.UtcNow.AddDays(10));
        _repository.GetAllAsync().Returns([earlyTask, lateTask]); // Intentionally ascending
        var query = new TodoQueryDto { SortBy = "duedate", SortDirection = "desc" };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.First().Title.Should().Be("Late");
    }

    [Fact]
    public async Task GetAllAsync_SortByPriorityDescending_ShouldReturnHighestFirst()
    {
        // Arrange
        var lowTask = TodoItem.Create("Low");
        lowTask.SetPriority(Priority.Low);
        var highTask = TodoItem.Create("High");
        highTask.SetPriority(Priority.High);
        _repository.GetAllAsync().Returns([lowTask, highTask]);
        var query = new TodoQueryDto { SortBy = "priority", SortDirection = "desc" };

        // Act
        var result = await _sut.GetAllAsync(query);

        // Assert
        result.Items.First().Priority.Should().Be(Priority.High);
    }

    // -------------------------------------------------------------------------
    // CreateAsync — with new fields
    // -------------------------------------------------------------------------

    [Fact]
    public async Task CreateAsync_WithAllEnhancedFields_ShouldPersistAndReturnDto()
    {
        // Arrange
        var due = DateTime.UtcNow.AddDays(5);
        var dto = new CreateTodoDto(
            Title: "Full task",
            Description: "A description",
            DueDate: due,
            Priority: Priority.High,
            Tags: ["work", "important"]);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.Description.Should().Be("A description");
        result.Priority.Should().Be(Priority.High);
        result.Tags.Should().BeEquivalentTo(["work", "important"]);
        result.DueDate.Should().BeCloseTo(due, TimeSpan.FromSeconds(1));
        await _repository.Received(1).AddAsync(Arg.Is<TodoItem>(t =>
            t.Description == "A description" &&
            t.Priority == Priority.High &&
            t.Tags.Contains("work") &&
            t.Tags.Contains("important")));
    }

    // -------------------------------------------------------------------------
    // UpdateAsync — with new fields
    // -------------------------------------------------------------------------

    [Fact]
    public async Task UpdateAsync_WithNewFields_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var item = TodoItem.Create("Task");
        _repository.GetByIdAsync(item.Id).Returns(item);
        var due = DateTime.UtcNow.AddDays(2);
        var dto = new UpdateTodoDto(
            Title: "Updated",
            IsCompleted: false,
            Description: "Updated desc",
            DueDate: due,
            Priority: Priority.Medium,
            Tags: ["new-tag"]);

        // Act
        var result = await _sut.UpdateAsync(item.Id, dto);

        // Assert
        result.Description.Should().Be("Updated desc");
        result.Priority.Should().Be(Priority.Medium);
        result.Tags.Should().Contain("new-tag");
        result.DueDate.Should().BeCloseTo(due, TimeSpan.FromSeconds(1));
    }

    // -------------------------------------------------------------------------
    // DeleteAsync — Soft Delete behaviour
    // -------------------------------------------------------------------------

    [Fact]
    public async Task DeleteAsync_WhenItemExists_ShouldSoftDeleteNotHardDelete()
    {
        // Arrange
        var item = TodoItem.Create("Task");
        _repository.GetByIdAsync(item.Id).Returns(item);

        // Act
        await _sut.DeleteAsync(item.Id);

        // Assert — entity state is soft-deleted
        item.IsDeleted.Should().BeTrue();
        // Update is called, NOT the hard-delete overload
        await _repository.Received(1).UpdateAsync(item);
        await _repository.DidNotReceive().DeleteAsync(Arg.Any<TodoItem>());
    }

    // -------------------------------------------------------------------------
    // RestoreAsync
    // -------------------------------------------------------------------------

    [Fact]
    public async Task RestoreAsync_WhenItemIsSoftDeleted_ShouldRestoreAndReturnDto()
    {
        // Arrange
        var item = TodoItem.Create("Task");
        item.SoftDelete();
        _repository.GetByIdIncludingDeletedAsync(item.Id).Returns(item);

        // Act
        var result = await _sut.RestoreAsync(item.Id);

        // Assert
        item.IsDeleted.Should().BeFalse();
        result.IsDeleted.Should().BeFalse();
        result.Id.Should().Be(item.Id);
        await _repository.Received(1).UpdateAsync(item);
    }

    [Fact]
    public async Task RestoreAsync_WhenItemNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdIncludingDeletedAsync(id).ReturnsNull();

        // Act
        var act = () => _sut.RestoreAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
