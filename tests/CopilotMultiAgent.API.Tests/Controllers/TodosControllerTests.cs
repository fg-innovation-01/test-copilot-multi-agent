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

public class TodosControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodosControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Evaluate db name ONCE here (not inside AddDbContext lambda which runs per scope)
                var dbName = "IntegrationTests-" + Guid.NewGuid();
                // Remove SqlServer provider configuration registered by AddInfrastructure
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                // Add InMemory provider instead, same DB name for all requests in this test
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetAll_WhenEmpty_ShouldReturn200WithEmptyPagedResult()
    {
        var response = await _client.GetAsync("/api/todos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<PagedResultDto<TodoDto>>();
        body.Should().NotBeNull();
        body!.Items.Should().BeEmpty();
        body.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Create_WithValidDto_ShouldReturn201WithCreatedItem()
    {
        var dto = new CreateTodoDto("Integration test task");
        var response = await _client.PostAsJsonAsync("/api/todos", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<TodoDto>();
        result.Should().NotBeNull();
        result!.Title.Should().Be("Integration test task");
        result.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetById_AfterCreate_ShouldReturnItem()
    {
        var dto = new CreateTodoDto("GetById test");
        var createResponse = await _client.PostAsJsonAsync("/api/todos", dto);
        var created = await createResponse.Content.ReadFromJsonAsync<TodoDto>();

        var getResponse = await _client.GetAsync($"/api/todos/{created!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await getResponse.Content.ReadFromJsonAsync<TodoDto>();
        result!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ShouldReturn404()
    {
        var response = await _client.GetAsync($"/api/todos/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_WithValidDto_ShouldReturn200WithUpdatedItem()
    {
        var created = await CreateTodoItemAsync("Before update");

        var updateDto = new UpdateTodoDto("After update", IsCompleted: true);
        var response = await _client.PutAsJsonAsync($"/api/todos/{created.Id}", updateDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TodoDto>();
        result!.Title.Should().Be("After update");
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturn204()
    {
        var created = await CreateTodoItemAsync("Delete me");

        var response = await _client.DeleteAsync($"/api/todos/{created.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/api/todos/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task<TodoDto> CreateTodoItemAsync(string title)
    {
        var response = await _client.PostAsJsonAsync("/api/todos", new CreateTodoDto(title));
        return (await response.Content.ReadFromJsonAsync<TodoDto>())!;
    }
}
