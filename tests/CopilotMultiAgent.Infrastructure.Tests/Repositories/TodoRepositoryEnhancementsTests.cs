using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Infrastructure.Persistence;
using CopilotMultiAgent.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CopilotMultiAgent.Infrastructure.Tests.Repositories;

public class TodoRepositoryEnhancementsTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly TodoRepository _sut;

    public TodoRepositoryEnhancementsTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _sut = new TodoRepository(_context);
    }

    // -------------------------------------------------------------------------
    // Global Query Filter — soft-deleted items are excluded by default
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAllAsync_ShouldNotReturnSoftDeletedItems()
    {
        // Arrange
        var visible = TodoItem.Create("Visible task");
        var deleted = TodoItem.Create("Deleted task");
        await _sut.AddAsync(visible);
        await _sut.AddAsync(deleted);
        deleted.SoftDelete();
        await _sut.UpdateAsync(deleted);

        // Act
        var items = await _sut.GetAllAsync();

        // Assert
        items.Should().ContainSingle(t => t.Id == visible.Id);
        items.Should().NotContain(t => t.Id == deleted.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSoftDeleted_ShouldReturnNull()
    {
        // Arrange
        var todo = TodoItem.Create("Deleted task");
        await _sut.AddAsync(todo);
        todo.SoftDelete();
        await _sut.UpdateAsync(todo);

        // Act
        var result = await _sut.GetByIdAsync(todo.Id);

        // Assert
        result.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // GetByIdIncludingDeletedAsync — bypasses the global filter
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetByIdIncludingDeletedAsync_WhenSoftDeleted_ShouldReturnItem()
    {
        // Arrange
        var todo = TodoItem.Create("Deleted task");
        await _sut.AddAsync(todo);
        todo.SoftDelete();
        await _sut.UpdateAsync(todo);

        // Act
        var result = await _sut.GetByIdIncludingDeletedAsync(todo.Id);

        // Assert
        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
        result.Id.Should().Be(todo.Id);
    }

    [Fact]
    public async Task GetByIdIncludingDeletedAsync_WhenItemExists_ShouldReturnItem()
    {
        // Arrange
        var todo = TodoItem.Create("Active task");
        await _sut.AddAsync(todo);

        // Act
        var result = await _sut.GetByIdIncludingDeletedAsync(todo.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(todo.Id);
    }

    [Fact]
    public async Task GetByIdIncludingDeletedAsync_WhenNotFound_ShouldReturnNull()
    {
        var result = await _sut.GetByIdIncludingDeletedAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    // -------------------------------------------------------------------------
    // Persistence of new fields
    // -------------------------------------------------------------------------

    [Fact]
    public async Task AddAsync_ShouldPersistPriorityField()
    {
        // Arrange
        var todo = TodoItem.Create("Task", priority: Priority.High);

        // Act
        await _sut.AddAsync(todo);

        // Assert
        var saved = await _context.TodoItems.FindAsync(todo.Id);
        saved!.Priority.Should().Be(Priority.High);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistDescriptionField()
    {
        // Arrange
        var todo = TodoItem.Create("Task", description: "Some description");

        // Act
        await _sut.AddAsync(todo);

        // Assert
        var saved = await _context.TodoItems.FindAsync(todo.Id);
        saved!.Description.Should().Be("Some description");
    }

    [Fact]
    public async Task AddAsync_ShouldPersistDueDateField()
    {
        // Arrange
        var due = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        var todo = TodoItem.Create("Task", dueDate: due);

        // Act
        await _sut.AddAsync(todo);

        // Assert
        var saved = await _context.TodoItems.FindAsync(todo.Id);
        saved!.DueDate.Should().Be(due);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistTagsField()
    {
        // Arrange
        var todo = TodoItem.Create("Task", tags: ["work", "important"]);

        // Act
        await _sut.AddAsync(todo);

        // Assert
        var saved = await _context.TodoItems.FindAsync(todo.Id);
        saved!.Tags.Should().BeEquivalentTo(["work", "important"]);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistSoftDeleteState()
    {
        // Arrange
        var todo = TodoItem.Create("Task");
        await _sut.AddAsync(todo);
        todo.SoftDelete();

        // Act
        await _sut.UpdateAsync(todo);

        // Use IgnoreQueryFilters to directly query the DB and verify the flag was saved
        var saved = await _context.TodoItems
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == todo.Id);
        saved!.IsDeleted.Should().BeTrue();
        saved.DeletedAt.Should().NotBeNull();
    }

    public void Dispose() => _context.Dispose();
}
