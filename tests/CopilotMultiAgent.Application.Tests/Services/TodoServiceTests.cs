using CopilotMultiAgent.Application.Common.Exceptions;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Interfaces.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace CopilotMultiAgent.Application.Tests.Services;

public class TodoServiceTests
{
    private readonly ITodoRepository _repository = Substitute.For<ITodoRepository>();
    private readonly TodoService _sut;

    public TodoServiceTests()
    {
        _sut = new TodoService(_repository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var items = new List<TodoItem>
        {
            TodoItem.Create("Task 1"),
            TodoItem.Create("Task 2")
        };
        _repository.GetAllAsync().Returns(items);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Select(r => r.Title).Should().BeEquivalentTo(["Task 1", "Task 2"]);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnDto()
    {
        // Arrange
        var item = TodoItem.Create("Task");
        _repository.GetByIdAsync(item.Id).Returns(item);

        // Act
        var result = await _sut.GetByIdAsync(item.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
        result.Title.Should().Be("Task");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = () => _sut.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddAndReturnDto()
    {
        // Arrange
        var dto = new CreateTodoDto("Buy milk");

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.Title.Should().Be("Buy milk");
        result.IsCompleted.Should().BeFalse();
        await _repository.Received(1).AddAsync(Arg.Is<TodoItem>(t => t.Title == "Buy milk"));
    }

    [Fact]
    public async Task UpdateAsync_WhenItemExists_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var item = TodoItem.Create("Old title");
        _repository.GetByIdAsync(item.Id).Returns(item);
        var dto = new UpdateTodoDto("New title", IsCompleted: true);

        // Act
        var result = await _sut.UpdateAsync(item.Id, dto);

        // Assert
        result.Title.Should().Be("New title");
        result.IsCompleted.Should().BeTrue();
        await _repository.Received(1).UpdateAsync(Arg.Any<TodoItem>());
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = () => _sut.UpdateAsync(id, new UpdateTodoDto("Title", false));

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenItemExists_ShouldSoftDeleteViaUpdate()
    {
        // Arrange
        var item = TodoItem.Create("Task");
        _repository.GetByIdAsync(item.Id).Returns(item);

        // Act
        await _sut.DeleteAsync(item.Id);

        // Assert — soft delete: entity flag set, persisted via UpdateAsync, hard-delete not called
        item.IsDeleted.Should().BeTrue();
        await _repository.Received(1).UpdateAsync(item);
        await _repository.DidNotReceive().DeleteAsync(Arg.Any<TodoItem>());
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = () => _sut.DeleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
