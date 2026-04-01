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

public class SortingControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SortingControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "SortingIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Sort_WithValidRequest_BubbleSort_Ascending_ShouldReturn200WithSortedNumbers()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.BubbleSort, SortDirection.Ascending);

        // Act
        var response = await _client.PostAsJsonAsync("/api/sorting", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SortResultDto>();
        result.Should().NotBeNull();
        result!.SortedNumbers.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public async Task Sort_WithMergeSort_Descending_ShouldReturn200WithDescendingNumbers()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.MergeSort, SortDirection.Descending);

        // Act
        var response = await _client.PostAsJsonAsync("/api/sorting", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SortResultDto>();
        result!.SortedNumbers.Should().Equal([5, 4, 3, 2, 1]);
    }

    [Fact]
    public async Task Sort_WithQuickSort_ShouldReturnAlgorithmUsedInResponse()
    {
        // Arrange
        var request = new SortRequestDto([2, 1], SortAlgorithm.QuickSort, SortDirection.Ascending);

        // Act
        var response = await _client.PostAsJsonAsync("/api/sorting", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SortResultDto>();
        result!.AlgorithmUsed.Should().Be("QuickSort");
    }

    [Fact]
    public async Task Sort_WithEmptyNumbers_ShouldReturn400()
    {
        // Arrange
        var request = new SortRequestDto([], SortAlgorithm.BubbleSort, SortDirection.Ascending);

        // Act
        var response = await _client.PostAsJsonAsync("/api/sorting", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sort_WithNullNumbers_ShouldReturn400()
    {
        // Arrange — send raw JSON with null numbers to bypass compile-time constraints
        var payload = new { numbers = (int[]?)null, algorithm = 0, direction = 0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/sorting", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sort_ResponseShouldContainElapsedMs()
    {
        // Arrange
        var request = new SortRequestDto([3, 1, 2], SortAlgorithm.MergeSort, SortDirection.Ascending);

        // Act
        var response = await _client.PostAsJsonAsync("/api/sorting", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SortResultDto>();
        result!.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
