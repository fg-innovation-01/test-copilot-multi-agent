using CopilotMultiAgent.Application.Services;
using CopilotMultiAgent.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace CopilotMultiAgent.API.Tests.Controllers;

public class ExceptionHandlingMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ExceptionHandlingMiddlewareTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldReturn500()
    {
        // Arrange — replace ITodoService with a mock that throws an unexpected exception
        var mockService = Substitute.For<ITodoService>();
        mockService.GetAllAsync(Arg.Any<Application.DTOs.TodoQueryDto>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Unexpected error"));

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbName = "ExMiddlewareTests-" + Guid.NewGuid();
                services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));

                // Replace the real service with one that throws
                services.RemoveAll(typeof(ITodoService));
                services.AddScoped<ITodoService>(_ => mockService);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/api/todos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
