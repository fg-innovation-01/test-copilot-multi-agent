using CopilotMultiAgent.API.Controllers;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CopilotMultiAgent.API.Tests.Controllers;

public class MinMaxControllerUnitTests
{
    [Fact]
    public void Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<IMinMaxService>();
        var controller = new MinMaxController(service);
        controller.ModelState.AddModelError("Numbers", "Numbers must not be empty.");
        var dto = new MinMaxRequestDto([]);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<IMinMaxService>();
        var dto = new MinMaxRequestDto([1, 2, 3]);
        service.Calculate(dto).Returns(new MinMaxResultDto(1, 3, 0));
        var controller = new MinMaxController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<MinMaxResultDto>();
    }
}
