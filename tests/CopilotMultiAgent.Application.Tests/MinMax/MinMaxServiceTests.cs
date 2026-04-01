using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.MinMax;

public class MinMaxServiceTests
{
    private readonly IMinMaxService _sut = new MinMaxService();

    [Fact]
    public void Calculate_ShouldReturnCorrectMinAndMax()
    {
        // Arrange
        var request = new MinMaxRequestDto([3, -1, 7, 0, 5]);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Min.Should().Be(-1);
        result.Max.Should().Be(7);
    }

    [Fact]
    public void Calculate_SingleElement_MinAndMaxAreEqual()
    {
        // Arrange
        var request = new MinMaxRequestDto([42]);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Min.Should().Be(42);
        result.Max.Should().Be(42);
    }

    [Fact]
    public void Calculate_ShouldMeasureElapsedMs()
    {
        // Arrange
        var request = new MinMaxRequestDto([3, -1, 7, 0, 5]);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
