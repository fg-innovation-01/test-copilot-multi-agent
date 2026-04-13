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

public class SphereVolumeControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SphereVolumeControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "SphereVolumeIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidRadius_ShouldReturn200WithVolume()
    {
        // Arrange
        var request = new SphereVolumeRequestDto(Radius: 3.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/spherevolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SphereVolumeResultDto>();
        result.Should().NotBeNull();
        // (4/3) * pi * r^3 = (4/3) * pi * 27
        result!.Volume.Should().BeApproximately((4.0 / 3.0) * Math.PI * 27.0, 0.001);
    }

    [Fact]
    public async Task Calculate_WithZeroRadius_ShouldReturn400()
    {
        // Arrange — radius 0 violates [Range(double.Epsilon, double.MaxValue)]
        var payload = new { radius = 0.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/spherevolume", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_ResponseShouldContainElapsedMs()
    {
        // Arrange
        var request = new SphereVolumeRequestDto(Radius: 1.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/spherevolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SphereVolumeResultDto>();
        result!.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
