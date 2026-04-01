using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Domain.Enums;
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

public class TodosControllerEnhancementsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodosControllerEnhancementsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "EnhancementsTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    // -------------------------------------------------------------------------
    // GET /api/todos — returns PagedResultDto
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAll_ShouldReturnPagedResult()
    {
        // Arrange
        await CreateTodoAsync("Task 1");
        await CreateTodoAsync("Task 2");

        // Act
        var response = await _client.GetAsync("/api/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();
        body.Should().NotBeNull();
        body!.Items.Should().HaveCountGreaterThanOrEqualTo(2);
        body.TotalCount.Should().BeGreaterThanOrEqualTo(2);
        body.Page.Should().Be(1);
    }

    // -------------------------------------------------------------------------
    // CREATE with enhanced fields
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Create_WithAllEnhancedFields_ShouldReturn201WithAllFields()
    {
        // Arrange
        var dto = new CreateTodoDto(
            Title: "Enhanced task",
            Description: "Some description",
            DueDate: new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
            Priority: Priority.High,
            Tags: ["api", "test"]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/todos", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<TodoDto>();
        result!.Description.Should().Be("Some description");
        result.Priority.Should().Be(Priority.High);
        result.Tags.Should().BeEquivalentTo(["api", "test"]);
        result.IsDeleted.Should().BeFalse();
    }

    // -------------------------------------------------------------------------
    // Filtering
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAll_FilterByPriority_ShouldReturnOnlyMatchingItems()
    {
        // Arrange
        await CreateTodoAsync("High task", priority: Priority.High);
        await CreateTodoAsync("Low task", priority: Priority.Low);

        // Act
        var response = await _client.GetAsync("/api/todos?priority=High");
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();

        // Assert
        body!.Items.Should().AllSatisfy(t => t.Priority.Should().Be(Priority.High));
        body.Items.Should().NotContain(t => t.Priority == Priority.Low);
    }

    [Fact]
    public async Task GetAll_FilterByIsCompleted_ShouldReturnOnlyCompletedItems()
    {
        // Arrange
        var created = await CreateTodoAsync("Completable task");
        await _client.PutAsJsonAsync($"/api/todos/{created.Id}",
            new UpdateTodoDto("Completable task", IsCompleted: true));
        await CreateTodoAsync("Active task");

        // Act
        var response = await _client.GetAsync("/api/todos?isCompleted=true");
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();

        // Assert
        body!.Items.Should().AllSatisfy(t => t.IsCompleted.Should().BeTrue());
    }

    [Fact]
    public async Task GetAll_FilterByTag_ShouldReturnOnlyItemsWithThatTag()
    {
        // Arrange
        await CreateTodoAsync("Work task", tags: ["work"]);
        await CreateTodoAsync("Personal task", tags: ["personal"]);

        // Act
        var response = await _client.GetAsync("/api/todos?tag=work");
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();

        // Assert
        body!.Items.Should().AllSatisfy(t => t.Tags.Should().Contain("work"));
        body.Items.Should().NotContain(t => t.Tags.Contains("personal"));
    }

    // -------------------------------------------------------------------------
    // Pagination
    // -------------------------------------------------------------------------

    [Fact]
    public async Task GetAll_WithPageSize_ShouldReturnCorrectNumberOfItems()
    {
        // Arrange
        for (int i = 1; i <= 5; i++)
            await CreateTodoAsync($"Paged task {i}");

        // Act
        var response = await _client.GetAsync("/api/todos?page=1&pageSize=2");
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();

        // Assert
        body!.Items.Should().HaveCount(2);
        body.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task GetAll_WithPage2_ShouldReturnSecondPageOfItems()
    {
        // Arrange — create exactly 3 todos so page 2 with pageSize=2 has 1 item
        for (int i = 1; i <= 3; i++)
            await CreateTodoAsync($"Page2 task {i}");

        // Act
        var response = await _client.GetAsync("/api/todos?page=2&pageSize=2");
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();

        // Assert
        body!.Page.Should().Be(2);
        body.TotalPages.Should().BeGreaterThanOrEqualTo(2);
    }

    // -------------------------------------------------------------------------
    // Soft Delete + Restore
    // -------------------------------------------------------------------------

    [Fact]
    public async Task Delete_ShouldSoftDelete_ItemNoLongerAppearsInGetAll()
    {
        // Arrange
        var created = await CreateTodoAsync("Soft delete me");

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/todos/{created.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Assert — item is gone from GetAll (soft-deleted by global filter)
        var listResponse = await _client.GetAsync("/api/todos");
        var body = await listResponse.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();
        body!.Items.Should().NotContain(t => t.Id == created.Id);
    }

    [Fact]
    public async Task Delete_ShouldSoftDelete_GetByIdReturns404()
    {
        // Arrange
        var created = await CreateTodoAsync("Soft delete me");

        // Act
        await _client.DeleteAsync($"/api/todos/{created.Id}");

        // Assert
        var getResponse = await _client.GetAsync($"/api/todos/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Restore_AfterSoftDelete_ShouldReturn200WithRestoredTodo()
    {
        // Arrange
        var created = await CreateTodoAsync("Restore me");
        await _client.DeleteAsync($"/api/todos/{created.Id}");

        // Act
        var response = await _client.PostAsync($"/api/todos/{created.Id}/restore", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TodoDto>();
        result!.Id.Should().Be(created.Id);
        result.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task Restore_AfterRestore_ItemAppearsInGetAll()
    {
        // Arrange
        var created = await CreateTodoAsync("Restore me");
        await _client.DeleteAsync($"/api/todos/{created.Id}");

        // Act
        await _client.PostAsync($"/api/todos/{created.Id}/restore", null);

        // Assert
        var listResponse = await _client.GetAsync("/api/todos");
        var body = await listResponse.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();
        body!.Items.Should().Contain(t => t.Id == created.Id);
    }

    [Fact]
    public async Task Restore_WhenItemNotFound_ShouldReturn404()
    {
        var response = await _client.PostAsync($"/api/todos/{Guid.NewGuid()}/restore", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private async Task<TodoDto> CreateTodoAsync(
        string title,
        Priority priority = Priority.Low,
        string[]? tags = null)
    {
        var dto = new CreateTodoDto(Title: title, Priority: priority, Tags: tags);
        var response = await _client.PostAsJsonAsync("/api/todos", dto);
        return (await response.Content.ReadFromJsonAsync<TodoDto>())!;
    }
}
