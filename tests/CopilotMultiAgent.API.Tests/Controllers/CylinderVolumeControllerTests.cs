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

public class CylinderVolumeControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CylinderVolumeControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "CylinderVolumeIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidRadiusAndHeight_ShouldReturn200WithVolume()
    {
        // Arrange
        var request = new CylinderVolumeRequestDto(Radius: 3.0, Height: 5.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/cylindervolume", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<CylinderVolumeResultDto>();
        result.Should().NotBeNull();
        result!.Volume.Should().BeApproximately(Math.PI * 9.0 * 5.0, 0.001);
    }

    [Fact]
    public async Task Calculate_WithZeroRadius_ShouldReturn400()
    {
        // Arrange — radius 0 violates [Range(double.Epsilon, double.MaxValue)]
        var payload = new { radius = 0.0, height = 5.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/cylindervolume", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_WithZeroHeight_ShouldReturn400()
    {
        // Arrange — height 0 violates [Range(double.Epsilon, double.MaxValue)]
        var payload = new { radius = 3.0, height = 0.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/cylindervolume", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
