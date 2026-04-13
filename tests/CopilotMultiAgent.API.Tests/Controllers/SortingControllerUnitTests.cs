using CopilotMultiAgent.API.Controllers;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using CopilotMultiAgent.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CopilotMultiAgent.API.Tests.Controllers;

public class SortingControllerUnitTests
{
    [Fact]
    public void Sort_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<ISortingService>();
        var controller = new SortingController(service);
        controller.ModelState.AddModelError("Numbers", "Numbers must not be empty.");
        var dto = new SortRequestDto([], SortAlgorithm.BubbleSort, SortDirection.Ascending);

        // Act
        var result = controller.Sort(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Sort_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<ISortingService>();
        var dto = new SortRequestDto([3, 1, 2], SortAlgorithm.BubbleSort, SortDirection.Ascending);
        service.Sort(dto).Returns(new SortResultDto([1, 2, 3], "BubbleSort", "Ascending", 0));
        var controller = new SortingController(service);

        // Act
        var result = controller.Sort(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
