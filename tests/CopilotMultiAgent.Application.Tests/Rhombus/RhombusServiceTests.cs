using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;
using Xunit;

namespace CopilotMultiAgent.Application.Tests.Rhombus;

public class RhombusServiceTests
{
    private readonly RhombusService _sut = new();

    [Fact]
    public void Calculate_WithValidRequest_ReturnsAreaPerimeterAndElapsedMs()
    {
        // Arrange
        var request = new RhombusRequestDto(Side: 5.0, Diagonal1: 6.0, Diagonal2: 8.0);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Should().NotBeNull();
        result.Area.Should().BeApproximately(24.0, 0.0001);
        result.Perimeter.Should().BeApproximately(20.0, 0.0001);
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Calculate_WithSide3Diagonal4And5_ReturnsCorrectAreaAndPerimeter()
    {
        // Arrange
        var request = new RhombusRequestDto(Side: 3.0, Diagonal1: 4.0, Diagonal2: 5.0);
        var expectedArea = 4.0 * 5.0 / 2.0;
        var expectedPerimeter = 4.0 * 3.0;

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Area.Should().BeApproximately(expectedArea, 0.0001);
        result.Perimeter.Should().BeApproximately(expectedPerimeter, 0.0001);
    }
}
