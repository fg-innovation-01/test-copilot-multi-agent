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

public class RhombusControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RhombusControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "RhombusIntegrationTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Calculate_WithValidRequest_ShouldReturn200WithAreaAndPerimeter()
    {
        // Arrange
        var request = new RhombusRequestDto(Side: 5.0, Diagonal1: 6.0, Diagonal2: 8.0);

        // Act
        var response = await _client.PostAsJsonAsync("/api/rhombus", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<RhombusResultDto>();
        result.Should().NotBeNull();
        result!.Area.Should().BeApproximately(24.0, 0.001);
        result.Perimeter.Should().BeApproximately(20.0, 0.001);
    }

    [Fact]
    public async Task Calculate_WithZeroSide_ShouldReturn400()
    {
        // Arrange
        var payload = new { side = 0.0, diagonal1 = 6.0, diagonal2 = 8.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rhombus", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_WithZeroDiagonal1_ShouldReturn400()
    {
        // Arrange
        var payload = new { side = 5.0, diagonal1 = 0.0, diagonal2 = 8.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rhombus", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Calculate_WithZeroDiagonal2_ShouldReturn400()
    {
        // Arrange
        var payload = new { side = 5.0, diagonal1 = 6.0, diagonal2 = 0.0 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rhombus", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
