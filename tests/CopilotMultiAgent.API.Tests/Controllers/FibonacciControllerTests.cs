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

public class FibonacciControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FibonacciControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "FibonacciIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidCount_ShouldReturn200WithSequence()
    {
        // Arrange
        var request = new FibonacciRequestDto(Count: 5);

        // Act
        var response = await _client.PostAsJsonAsync("/api/fibonacci", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FibonacciResultDto>();
        result.Should().NotBeNull();
        result!.Sequence.Should().HaveCount(5);
        result!.Sequence.Should().BeEquivalentTo(new long[] { 0, 1, 1, 2, 3 }, opts => opts.WithStrictOrdering());
    }

    [Fact]
    public async Task Calculate_WithCountZero_ShouldReturn400()
    {
        // Arrange — count 0 violates [Range(1, 93)]
        var payload = new { count = 0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/fibonacci", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_WithCountExceedingMax_ShouldReturn400()
    {
        // Arrange — count 94 violates [Range(1, 93)]
        var payload = new { count = 94 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/fibonacci", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_ResponseShouldContainElapsedMs()
    {
        // Arrange
        var request = new FibonacciRequestDto(Count: 3);

        // Act
        var response = await _client.PostAsJsonAsync("/api/fibonacci", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FibonacciResultDto>();
        result!.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
