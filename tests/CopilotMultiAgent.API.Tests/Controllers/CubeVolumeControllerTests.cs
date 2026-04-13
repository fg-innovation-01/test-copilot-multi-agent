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

public class CubeVolumeControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CubeVolumeControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "CubeVolumeIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidSideLength_ShouldReturn200WithVolume()
    {
        // Arrange
        var request = new CubeVolumeRequestDto(3.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/cubevolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CubeVolumeResultDto>();
        result.Should().NotBeNull();
        result!.Volume.Should().BeApproximately(27.0, 0.0001);
    }

    [Fact]
    public async Task Calculate_WithZeroSideLength_ShouldReturn400()
    {
        // Arrange — side length 0 violates [Range(double.Epsilon, double.MaxValue)]
        var payload = new { sideLength = 0.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/cubevolume", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_ResponseShouldContainElapsedMs()
    {
        // Arrange
        var request = new CubeVolumeRequestDto(2.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/cubevolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CubeVolumeResultDto>();
        result!.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
