using CopilotMultiAgent.Application.CubeVolume;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.CubeVolume;

public class CubeVolumeAlgorithmTests
{
    private readonly CubeVolumeAlgorithm _sut = new();

    [Fact]
    public void Calculate_WithSideLength5_ReturnsCorrectVolume()
    {
        // Arrange
        var sideLength = 5.0;
        var expected = Math.Pow(5, 3);

        // Act
        var result = _sut.Calculate(sideLength);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithSideLength1_ReturnsCorrectVolume()
    {
        // Arrange
        var sideLength = 1.0;
        var expected = 1.0;

        // Act
        var result = _sut.Calculate(sideLength);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithLargeSideLength_ReturnsCorrectVolume()
    {
        // Arrange
        var sideLength = 1000.0;
        var expected = Math.Pow(1000, 3);

        // Act
        var result = _sut.Calculate(sideLength);

        // Assert
        result.Should().BeApproximately(expected, 0.01);
    }

    [Fact]
    public void Calculate_WithDecimalSideLength_ReturnsCorrectVolume()
    {
        // Arrange
        var sideLength = 2.5;
        var expected = Math.Pow(2.5, 3);

        // Act
        var result = _sut.Calculate(sideLength);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithVerySmallSideLength_ReturnsCorrectVolume()
    {
        // Arrange
        var sideLength = 0.001;
        var expected = Math.Pow(0.001, 3);

        // Act
        var result = _sut.Calculate(sideLength);

        // Assert
        result.Should().BeApproximately(expected, 1e-12);
    }

    [Fact]
    public void Calculate_WithZeroSideLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var sideLength = 0.0;

        // Act
        var act = () => _sut.Calculate(sideLength);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("sideLength");
    }

    [Fact]
    public void Calculate_WithNegativeSideLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var sideLength = -3.0;

        // Act
        var act = () => _sut.Calculate(sideLength);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("sideLength");
    }
}
