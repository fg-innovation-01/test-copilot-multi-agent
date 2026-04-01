using CopilotMultiAgent.Domain.Entities;
using CopilotMultiAgent.Infrastructure.Persistence;
using CopilotMultiAgent.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CopilotMultiAgent.Infrastructure.Tests.Repositories;

public class ScheduleRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ScheduleRepository _sut;

    public ScheduleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _sut = new ScheduleRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistItem()
    {
        // Arrange
        var schedule = Schedule.Create("Team meeting", DateTime.UtcNow.AddDays(1));

        // Act
        await _sut.AddAsync(schedule);

        // Assert
        var saved = await _context.Schedules.FindAsync(schedule.Id);
        saved.Should().NotBeNull();
        saved!.Title.Should().Be("Team meeting");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllNonDeletedItems()
    {
        // Arrange
        var active = Schedule.Create("Active", DateTime.UtcNow.AddDays(1));
        var deleted = Schedule.Create("Deleted", DateTime.UtcNow.AddDays(2));
        deleted.SoftDelete();

        await _sut.AddAsync(active);
        await _context.Schedules.AddAsync(deleted);
        await _context.SaveChangesAsync();

        // Act
        var items = await _sut.GetAllAsync();

        // Assert
        items.Should().HaveCount(1);
        items.First().Title.Should().Be("Active");
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnItem()
    {
        // Arrange
        var schedule = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        await _sut.AddAsync(schedule);

        // Act
        var result = await _sut.GetByIdAsync(schedule.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(schedule.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenDeleted_ShouldReturnNull()
    {
        // Arrange
        var schedule = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        schedule.SoftDelete();
        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(schedule.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges()
    {
        // Arrange
        var schedule = Schedule.Create("Original", DateTime.UtcNow.AddDays(1));
        await _sut.AddAsync(schedule);
        schedule.UpdateTitle("Updated");

        // Act
        await _sut.UpdateAsync(schedule);

        // Assert
        var updated = await _context.Schedules.FindAsync(schedule.Id);
        updated!.Title.Should().Be("Updated");
    }

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyList()
    {
        // Act
        var items = await _sut.GetAllAsync();

        // Assert
        items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveItem()
    {
        // Arrange
        var schedule = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        await _sut.AddAsync(schedule);

        // Act
        await _sut.DeleteAsync(schedule);

        // Assert
        var deleted = await _context.Schedules.IgnoreQueryFilters().FirstOrDefaultAsync(s => s.Id == schedule.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldPersistScheduledAt()
    {
        // Arrange
        var scheduledAt = new DateTime(2026, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var schedule = Schedule.Create("Meeting", scheduledAt);

        // Act
        await _sut.AddAsync(schedule);

        // Assert
        var saved = await _context.Schedules.FindAsync(schedule.Id);
        saved!.ScheduledAt.Should().Be(scheduledAt);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
