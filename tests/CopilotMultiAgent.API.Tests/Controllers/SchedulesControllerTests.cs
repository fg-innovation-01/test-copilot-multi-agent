using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;
using System.Net.Http.Json;

namespace CopilotMultiAgent.API.Tests.Controllers;

public class SchedulesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SchedulesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "ScheduleIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetAll_WhenEmpty_ShouldReturn200WithEmptyList()
    {
        // Act
        var response = await _client.GetAsync("/api/schedules");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IEnumerable<ScheduleDto>>();
        body.Should().NotBeNull();
        body!.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_WithValidDto_ShouldReturn201WithCreatedItem()
    {
        // Arrange
        var dto = new CreateScheduleDto("Daily standup", DateTime.UtcNow.AddDays(1));

        // Act
        var response = await _client.PostAsJsonAsync("/api/schedules", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<ScheduleDto>();
        result.Should().NotBeNull();
        result!.Title.Should().Be("Daily standup");
        result.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetById_WhenExists_ShouldReturn200()
    {
        // Arrange
        var createDto = new CreateScheduleDto("Review meeting", DateTime.UtcNow.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/schedules", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<ScheduleDto>();

        // Act
        var response = await _client.GetAsync($"/api/schedules/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.GetAsync($"/api/schedules/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenExists_ShouldReturn204()
    {
        // Arrange
        var dto = new CreateScheduleDto("Meeting to delete", DateTime.UtcNow.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/schedules", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<ScheduleDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/schedules/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Complete_WhenExists_ShouldReturn200WithCompletedItem()
    {
        // Arrange
        var dto = new CreateScheduleDto("Meeting to complete", DateTime.UtcNow.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/schedules", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<ScheduleDto>();

        // Act
        var response = await _client.PostAsync($"/api/schedules/{created!.Id}/complete", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ScheduleDto>();
        result!.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task Update_WhenExists_ShouldReturn200WithUpdatedTitle()
    {
        // Arrange
        var createDto = new CreateScheduleDto("Original title", DateTime.UtcNow.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/schedules", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<ScheduleDto>();
        var updateDto = new UpdateScheduleDto("Updated title", DateTime.UtcNow.AddDays(2), Domain.Enums.RecurrenceType.Weekly, false);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/schedules/{created!.Id}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ScheduleDto>();
        result!.Title.Should().Be("Updated title");
        result.RecurrenceType.Should().Be(Domain.Enums.RecurrenceType.Weekly);
    }

    [Fact]
    public async Task Update_WhenNotFound_ShouldReturn404()
    {
        // Arrange
        var dto = new UpdateScheduleDto("Title", DateTime.UtcNow.AddDays(1), Domain.Enums.RecurrenceType.None, false);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/schedules/{Guid.NewGuid()}", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/schedules/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Complete_WhenNotFound_ShouldReturn404()
    {
        // Act
        var response = await _client.PostAsync($"/api/schedules/{Guid.NewGuid()}/complete", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Complete_WhenAlreadyCompleted_ShouldReturn400()
    {
        // Arrange
        var dto = new CreateScheduleDto("Meeting", DateTime.UtcNow.AddDays(1));
        var createResponse = await _client.PostAsJsonAsync("/api/schedules", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<ScheduleDto>();
        await _client.PostAsync($"/api/schedules/{created!.Id}/complete", null);

        // Act — complete again
        var response = await _client.PostAsync($"/api/schedules/{created.Id}/complete", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_WithMissingTitle_ShouldReturn400()
    {
        // Arrange — send object with empty title to trigger DataAnnotation validation
        var dto = new { Title = "", ScheduledAt = DateTime.UtcNow.AddDays(1) };

        // Act
        var response = await _client.PostAsJsonAsync("/api/schedules", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
