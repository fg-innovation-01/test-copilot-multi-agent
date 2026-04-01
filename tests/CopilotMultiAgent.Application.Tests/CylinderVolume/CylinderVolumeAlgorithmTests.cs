using CopilotMultiAgent.Application.CylinderVolume;
using FluentAssertions;
using Xunit;

namespace CopilotMultiAgent.Application.Tests.CylinderVolume;

public class CylinderVolumeAlgorithmTests
{
    private readonly CylinderVolumeAlgorithm _sut = new();

    [Fact]
    public void Calculate_WithRadius3AndHeight5_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 3.0;
        var height = 5.0;
        var expected = Math.PI * Math.Pow(radius, 2) * height;

        // Act
        var result = _sut.Calculate(radius, height);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithRadius1AndHeight1_ReturnsPi()
    {
        // Arrange
        var radius = 1.0;
        var height = 1.0;
        var expected = Math.PI;

        // Act
        var result = _sut.Calculate(radius, height);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithZeroRadius_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var radius = 0.0;
        var height = 5.0;

        // Act
        var act = () => _sut.Calculate(radius, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_WithNegativeRadius_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var radius = -1.0;
        var height = 5.0;

        // Act
        var act = () => _sut.Calculate(radius, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_WithZeroHeight_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var radius = 3.0;
        var height = 0.0;

        // Act
        var act = () => _sut.Calculate(radius, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_WithNegativeHeight_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var radius = 3.0;
        var height = -5.0;

        // Act
        var act = () => _sut.Calculate(radius, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_WithLargeValues_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 1000.0;
        var height = 2000.0;
        var expected = Math.PI * Math.Pow(radius, 2) * height;

        // Act
        var result = _sut.Calculate(radius, height);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }
}