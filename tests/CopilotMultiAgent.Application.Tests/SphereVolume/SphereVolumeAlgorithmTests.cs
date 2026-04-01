using CopilotMultiAgent.Application.SphereVolume;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.SphereVolume;

public class SphereVolumeAlgorithmTests
{
    private readonly SphereVolumeAlgorithm _sut = new();

    [Fact]
    public void Calculate_WithRadius5_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 5.0;
        var expected = (4.0 / 3.0) * Math.PI * Math.Pow(5, 3);

        // Act
        var result = _sut.Calculate(radius);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithRadius1_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 1.0;
        var expected = (4.0 / 3.0) * Math.PI;

        // Act
        var result = _sut.Calculate(radius);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithLargeRadius_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 1000.0;
        var expected = (4.0 / 3.0) * Math.PI * Math.Pow(1000, 3);

        // Act
        var result = _sut.Calculate(radius);

        // Assert
        result.Should().BeApproximately(expected, 0.01);
    }

    [Fact]
    public void Calculate_WithDecimalRadius_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 2.5;
        var expected = (4.0 / 3.0) * Math.PI * Math.Pow(2.5, 3);

        // Act
        var result = _sut.Calculate(radius);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithVerySmallRadius_ReturnsCorrectVolume()
    {
        // Arrange
        var radius = 0.001;
        var expected = (4.0 / 3.0) * Math.PI * Math.Pow(0.001, 3);

        // Act
        var result = _sut.Calculate(radius);

        // Assert
        result.Should().BeApproximately(expected, 1e-12);
    }

    [Fact]
    public void Calculate_WithZeroRadius_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var radius = 0.0;

        // Act
        var act = () => _sut.Calculate(radius);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Calculate_WithNegativeRadius_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var radius = -5.0;

        // Act
        var act = () => _sut.Calculate(radius);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
