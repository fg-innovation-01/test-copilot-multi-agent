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

public class TriangularPyramidVolumeControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TriangularPyramidVolumeControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "TriangularPyramidIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidBaseEdgeAndHeight_ShouldReturn200WithVolume()
    {
        // Arrange
        var request = new TriangularPyramidVolumeRequestDto(BaseEdge: 4.0, Height: 6.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/triangularpyramidvolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TriangularPyramidVolumeResultDto>();
        result.Should().NotBeNull();
        result!.Volume.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Calculate_WithZeroBaseEdge_ShouldReturn400()
    {
        // Arrange — base edge 0 violates [Range(double.Epsilon, double.MaxValue)]
        var payload = new { baseEdge = 0.0, height = 6.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/triangularpyramidvolume", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_WithZeroHeight_ShouldReturn400()
    {
        // Arrange — height 0 violates [Range(double.Epsilon, double.MaxValue)]
        var payload = new { baseEdge = 4.0, height = 0.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/triangularpyramidvolume", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_ResponseShouldContainElapsedMs()
    {
        // Arrange
        var request = new TriangularPyramidVolumeRequestDto(BaseEdge: 3.0, Height: 4.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/triangularpyramidvolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<TriangularPyramidVolumeResultDto>();
        result!.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
