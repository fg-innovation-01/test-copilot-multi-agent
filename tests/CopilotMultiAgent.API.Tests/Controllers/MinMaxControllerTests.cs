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

public class MinMaxControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MinMaxControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "MinMaxIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidRequest_ShouldReturn200WithMinAndMax()
    {
        // Arrange
        var request = new MinMaxRequestDto([3, -1, 7, 0, 5]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/minmax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<MinMaxResultDto>();
        result.Should().NotBeNull();
        result!.Min.Should().Be(-1);
        result!.Max.Should().Be(7);
    }

    [Fact]
    public async Task Calculate_WithSingleElement_ShouldReturn200()
    {
        // Arrange
        var request = new MinMaxRequestDto([42]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/minmax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<MinMaxResultDto>();
        result.Should().NotBeNull();
        result!.Min.Should().Be(42);
        result!.Max.Should().Be(42);
    }

    [Fact]
    public async Task Calculate_WithEmptyNumbers_ShouldReturn400()
    {
        // Arrange
        var request = new MinMaxRequestDto([]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/minmax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_WithNullNumbers_ShouldReturn400()
    {
        // Arrange — send raw JSON with null numbers to bypass compile-time constraints
        var payload = new { numbers = (int[]?)null };

        // Act
        var response = await _client.PostAsJsonAsync("/api/minmax", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_ResponseShouldContainElapsedMs()
    {
        // Arrange
        var request = new MinMaxRequestDto([3, -1, 7]);

        // Act
        var response = await _client.PostAsJsonAsync("/api/minmax", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<MinMaxResultDto>();
        result!.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
