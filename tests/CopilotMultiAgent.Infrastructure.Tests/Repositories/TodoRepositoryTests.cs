using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Infrastructure.Persistence;
using CopilotMultiAgent.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CopilotMultiAgent.Infrastructure.Tests.Repositories;

public class TodoRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly TodoRepository _sut;

    public TodoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _sut = new TodoRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistItem()
    {
        var todo = TodoItem.Create("Test task");
        await _sut.AddAsync(todo);

        var saved = await _context.TodoItems.FindAsync(todo.Id);
        saved.Should().NotBeNull();
        saved!.Title.Should().Be("Test task");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        await _sut.AddAsync(TodoItem.Create("Task A"));
        await _sut.AddAsync(TodoItem.Create("Task B"));

        var items = await _sut.GetAllAsync();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnItem()
    {
        var todo = TodoItem.Create("Find me");
        await _sut.AddAsync(todo);

        var result = await _sut.GetByIdAsync(todo.Id);
        result.Should().NotBeNull();
        result!.Id.Should().Be(todo.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnNull()
    {
        var result = await _sut.GetByIdAsync(Guid.NewGuid());
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        var todo = TodoItem.Create("Original");
        await _sut.AddAsync(todo);

        todo.UpdateTitle("Updated");
        await _sut.UpdateAsync(todo);

        var updated = await _context.TodoItems.FindAsync(todo.Id);
        updated!.Title.Should().Be("Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveItem()
    {
        var todo = TodoItem.Create("Delete me");
        await _sut.AddAsync(todo);

        await _sut.DeleteAsync(todo);

        var deleted = await _context.TodoItems.FindAsync(todo.Id);
        deleted.Should().BeNull();
    }

    public void Dispose() => _context.Dispose();
}
