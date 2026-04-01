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

public class ScheduleServiceTests
{
    private readonly IScheduleRepository _repository = Substitute.For<IScheduleRepository>();
    private readonly ScheduleService _sut;

    public ScheduleServiceTests()
    {
        _sut = new ScheduleService(_repository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var items = new List<Schedule>
        {
            Schedule.Create("Meeting 1", DateTime.UtcNow.AddDays(1)),
            Schedule.Create("Meeting 2", DateTime.UtcNow.AddDays(2))
        };
        _repository.GetAllAsync().Returns(items);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Select(r => r.Title).Should().BeEquivalentTo(["Meeting 1", "Meeting 2"]);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnDto()
    {
        // Arrange
        var item = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        _repository.GetByIdAsync(item.Id).Returns(item);

        // Act
        var result = await _sut.GetByIdAsync(item.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
        result.Title.Should().Be("Meeting");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = async () => await _sut.GetByIdAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddAndReturnDto()
    {
        // Arrange
        var dto = new CreateScheduleDto("Team standup", DateTime.UtcNow.AddDays(1));

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.Title.Should().Be("Team standup");
        result.IsCompleted.Should().BeFalse();
        await _repository.Received(1).AddAsync(Arg.Is<Schedule>(s => s.Title == "Team standup"));
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var item = Schedule.Create("Old title", DateTime.UtcNow.AddDays(1));
        _repository.GetByIdAsync(item.Id).Returns(item);
        var newDate = DateTime.UtcNow.AddDays(5);
        var dto = new UpdateScheduleDto("New title", newDate, RecurrenceType.Weekly, false);

        // Act
        var result = await _sut.UpdateAsync(item.Id, dto);

        // Assert
        result.Title.Should().Be("New title");
        await _repository.Received(1).UpdateAsync(Arg.Is<Schedule>(s => s.Title == "New title"));
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();
        var dto = new UpdateScheduleDto("Title", DateTime.UtcNow.AddDays(1), RecurrenceType.None, false);

        // Act
        var act = async () => await _sut.UpdateAsync(id, dto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldSoftDeleteItem()
    {
        // Arrange
        var item = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        _repository.GetByIdAsync(item.Id).Returns(item);

        // Act
        await _sut.DeleteAsync(item.Id);

        // Assert
        await _repository.Received(1).UpdateAsync(Arg.Is<Schedule>(s => s.IsDeleted));
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = async () => await _sut.DeleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CompleteAsync_WhenExists_ShouldMarkCompleted()
    {
        // Arrange
        var item = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        _repository.GetByIdAsync(item.Id).Returns(item);

        // Act
        var result = await _sut.CompleteAsync(item.Id);

        // Assert
        result.IsCompleted.Should().BeTrue();
        await _repository.Received(1).UpdateAsync(Arg.Is<Schedule>(s => s.IsCompleted));
    }

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        _repository.GetAllAsync().Returns([]);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task CompleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).ReturnsNull();

        // Act
        var act = async () => await _sut.CompleteAsync(id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ShouldSetRecurrenceType()
    {
        // Arrange
        var dto = new CreateScheduleDto("Weekly sync", DateTime.UtcNow.AddDays(1), RecurrenceType.Weekly);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.RecurrenceType.Should().Be(RecurrenceType.Weekly);
        await _repository.Received(1).AddAsync(Arg.Is<Schedule>(s => s.RecurrenceType == RecurrenceType.Weekly));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateScheduledAtAndRecurrenceType()
    {
        // Arrange
        var item = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        _repository.GetByIdAsync(item.Id).Returns(item);
        var newDate = DateTime.UtcNow.AddDays(30);
        var dto = new UpdateScheduleDto("Meeting", newDate, RecurrenceType.Monthly, false);

        // Act
        var result = await _sut.UpdateAsync(item.Id, dto);

        // Assert
        result.ScheduledAt.Should().Be(newDate);
        result.RecurrenceType.Should().Be(RecurrenceType.Monthly);
    }

    [Fact]
    public async Task UpdateAsync_WhenAlreadyCompleted_SetToFalse_ShouldCallReopen()
    {
        // Arrange
        var item = Schedule.Create("Meeting", DateTime.UtcNow.AddDays(1));
        item.Complete();
        _repository.GetByIdAsync(item.Id).Returns(item);
        var dto = new UpdateScheduleDto("Meeting", item.ScheduledAt, RecurrenceType.None, false);

        // Act
        var result = await _sut.UpdateAsync(item.Id, dto);

        // Assert
        result.IsCompleted.Should().BeFalse();
        await _repository.Received(1).UpdateAsync(Arg.Is<Schedule>(s => !s.IsCompleted));
    }
}
