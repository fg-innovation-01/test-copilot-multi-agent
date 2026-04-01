using CopilotMultiAgent.Application.TriangularPyramidVolume;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.TriangularPyramidVolume;

public class TriangularPyramidVolumeAlgorithmTests
{
    private readonly TriangularPyramidVolumeAlgorithm _sut = new();

    [Fact]
    public void Calculate_WithBaseEdge3AndHeight5_ReturnsCorrectVolume()
    {
        // Arrange
        var baseEdge = 3.0;
        var height = 5.0;
        var expected = Math.Sqrt(3) / 12.0 * Math.Pow(baseEdge, 2) * height;

        // Act
        var result = _sut.Calculate(baseEdge, height);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithBaseEdge1AndHeight1_ReturnsCorrectVolume()
    {
        // Arrange
        var baseEdge = 1.0;
        var height = 1.0;
        var expected = Math.Sqrt(3) / 12.0;

        // Act
        var result = _sut.Calculate(baseEdge, height);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithLargeValues_ReturnsCorrectVolume()
    {
        // Arrange
        var baseEdge = 1000.0;
        var height = 2000.0;
        var expected = Math.Sqrt(3) / 12.0 * Math.Pow(1000, 2) * 2000;

        // Act
        var result = _sut.Calculate(baseEdge, height);

        // Assert
        result.Should().BeApproximately(expected, 0.01);
    }

    [Fact]
    public void Calculate_WithDecimalValues_ReturnsCorrectVolume()
    {
        // Arrange
        var baseEdge = 2.5;
        var height = 3.7;
        var expected = Math.Sqrt(3) / 12.0 * Math.Pow(2.5, 2) * 3.7;

        // Act
        var result = _sut.Calculate(baseEdge, height);

        // Assert
        result.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithVerySmallValues_ReturnsCorrectVolume()
    {
        // Arrange
        var baseEdge = 0.001;
        var height = 0.001;
        var expected = Math.Sqrt(3) / 12.0 * Math.Pow(0.001, 2) * 0.001;

        // Act
        var result = _sut.Calculate(baseEdge, height);

        // Assert
        result.Should().BeApproximately(expected, 1e-12);
    }

    [Fact]
    public void Calculate_WithZeroBaseEdge_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var baseEdge = 0.0;
        var height = 5.0;

        // Act
        var act = () => _sut.Calculate(baseEdge, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("baseEdge");
    }

    [Fact]
    public void Calculate_WithNegativeBaseEdge_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var baseEdge = -3.0;
        var height = 5.0;

        // Act
        var act = () => _sut.Calculate(baseEdge, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("baseEdge");
    }

    [Fact]
    public void Calculate_WithZeroHeight_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var baseEdge = 3.0;
        var height = 0.0;

        // Act
        var act = () => _sut.Calculate(baseEdge, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("height");
    }

    [Fact]
    public void Calculate_WithNegativeHeight_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var baseEdge = 3.0;
        var height = -5.0;

        // Act
        var act = () => _sut.Calculate(baseEdge, height);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("height");
    }
}
