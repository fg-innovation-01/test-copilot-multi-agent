using CopilotMultiAgent.API.Controllers;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
namespace CopilotMultiAgent.API.Tests.Controllers;

public class VolumeControllersUnitTests
{
    // ── CubeVolumeController ──────────────────────────────────────────────

    [Fact]
    public void CubeVolumeController_Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<ICubeVolumeService>();
        var controller = new CubeVolumeController(service);
        controller.ModelState.AddModelError("SideLength", "Side length must be greater than zero.");
        var dto = new CubeVolumeRequestDto(0);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void CubeVolumeController_Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<ICubeVolumeService>();
        var dto = new CubeVolumeRequestDto(3.0);
        service.Calculate(dto).Returns(new CubeVolumeResultDto(27.0, 0));
        var controller = new CubeVolumeController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    // ── CylinderVolumeController ──────────────────────────────────────────

    [Fact]
    public void CylinderVolumeController_Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<ICylinderVolumeService>();
        var controller = new CylinderVolumeController(service);
        controller.ModelState.AddModelError("Radius", "Radius must be greater than zero.");
        var dto = new CylinderVolumeRequestDto(0, 5.0);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void CylinderVolumeController_Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<ICylinderVolumeService>();
        var dto = new CylinderVolumeRequestDto(3.0, 5.0);
        service.Calculate(dto).Returns(new CylinderVolumeResultDto(141.37, 0));
        var controller = new CylinderVolumeController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    // ── SphereVolumeController ────────────────────────────────────────────

    [Fact]
    public void SphereVolumeController_Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<ISphereVolumeService>();
        var controller = new SphereVolumeController(service);
        controller.ModelState.AddModelError("Radius", "Radius must be greater than zero.");
        var dto = new SphereVolumeRequestDto(0);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void SphereVolumeController_Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<ISphereVolumeService>();
        var dto = new SphereVolumeRequestDto(3.0);
        service.Calculate(dto).Returns(new SphereVolumeResultDto(113.1, 0));
        var controller = new SphereVolumeController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    // ── TriangularPyramidVolumeController ─────────────────────────────────

    [Fact]
    public void TriangularPyramidVolumeController_Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<ITriangularPyramidVolumeService>();
        var controller = new TriangularPyramidVolumeController(service);
        controller.ModelState.AddModelError("BaseEdge", "Base edge must be greater than zero.");
        var dto = new TriangularPyramidVolumeRequestDto(0, 6.0);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void TriangularPyramidVolumeController_Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<ITriangularPyramidVolumeService>();
        var dto = new TriangularPyramidVolumeRequestDto(4.0, 6.0);
        service.Calculate(dto).Returns(new TriangularPyramidVolumeResultDto(13.86, 0));
        var controller = new TriangularPyramidVolumeController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    // ── FibonacciController ───────────────────────────────────────────────

    [Fact]
    public void FibonacciController_Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<IFibonacciService>();
        var controller = new FibonacciController(service);
        controller.ModelState.AddModelError("Count", "Count must be between 1 and 93.");
        var dto = new FibonacciRequestDto(0);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void FibonacciController_Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<IFibonacciService>();
        var dto = new FibonacciRequestDto(3);
        service.Calculate(dto).Returns(new FibonacciResultDto(new long[] { 1, 1, 2 }, 0));
        var controller = new FibonacciController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    // ── RhombusController ─────────────────────────────────────────────────

    [Fact]
    public void RhombusController_Calculate_WithInvalidModelState_ShouldReturnBadRequest()
    {
        // Arrange
        var service = Substitute.For<IRhombusService>();
        var controller = new RhombusController(service);
        controller.ModelState.AddModelError("Side", "Side must be greater than zero.");
        var dto = new RhombusRequestDto(0, 6.0, 8.0);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void RhombusController_Calculate_WithValidDto_ShouldReturnOk()
    {
        // Arrange
        var service = Substitute.For<IRhombusService>();
        var dto = new RhombusRequestDto(5.0, 6.0, 8.0);
        service.Calculate(dto).Returns(new RhombusResultDto(24.0, 20.0, 0));
        var controller = new RhombusController(service);

        // Act
        var result = controller.Calculate(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}
