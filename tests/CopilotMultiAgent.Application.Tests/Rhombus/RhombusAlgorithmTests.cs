using CopilotMultiAgent.Application.Rhombus;
using FluentAssertions;
using Xunit;

namespace CopilotMultiAgent.Application.Tests.Rhombus;

public class RhombusAlgorithmTests
{
    private readonly RhombusAlgorithm _sut = new();

    // Happy Path — CalculateArea

    [Fact]
    public void CalculateArea_WithDiagonal6And8_Returns24()
    {
        // Arrange
        var diagonal1 = 6.0;
        var diagonal2 = 8.0;

        // Act
        var result = _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        result.Should().BeApproximately(24.0, 0.0001);
    }

    [Fact]
    public void CalculateArea_WithDiagonal4And3_Returns6()
    {
        // Arrange
        var diagonal1 = 4.0;
        var diagonal2 = 3.0;

        // Act
        var result = _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        result.Should().BeApproximately(6.0, 0.0001);
    }

    // Happy Path — CalculatePerimeter

    [Fact]
    public void CalculatePerimeter_WithSide5_Returns20()
    {
        // Arrange
        var side = 5.0;

        // Act
        var result = _sut.CalculatePerimeter(side);

        // Assert
        result.Should().BeApproximately(20.0, 0.0001);
    }

    [Fact]
    public void CalculatePerimeter_WithSide2Point5_Returns10()
    {
        // Arrange
        var side = 2.5;

        // Act
        var result = _sut.CalculatePerimeter(side);

        // Assert
        result.Should().BeApproximately(10.0, 0.0001);
    }

    // Sad Path — CalculateArea guard clauses

    [Fact]
    public void CalculateArea_WithZeroDiagonal1_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var diagonal1 = 0.0;
        var diagonal2 = 8.0;

        // Act
        var act = () => _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CalculateArea_WithNegativeDiagonal1_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var diagonal1 = -1.0;
        var diagonal2 = 8.0;

        // Act
        var act = () => _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CalculateArea_WithZeroDiagonal2_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var diagonal1 = 6.0;
        var diagonal2 = 0.0;

        // Act
        var act = () => _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CalculateArea_WithNegativeDiagonal2_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var diagonal1 = 6.0;
        var diagonal2 = -1.0;

        // Act
        var act = () => _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    // Sad Path — CalculatePerimeter guard clauses

    [Fact]
    public void CalculatePerimeter_WithZeroSide_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var side = 0.0;

        // Act
        var act = () => _sut.CalculatePerimeter(side);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CalculatePerimeter_WithNegativeSide_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var side = -5.0;

        // Act
        var act = () => _sut.CalculatePerimeter(side);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    // Edge Cases

    [Fact]
    public void CalculateArea_WithVerySmallDiagonals_ReturnsCorrectArea()
    {
        // Arrange
        var diagonal1 = 0.001;
        var diagonal2 = 0.001;
        var expected = 0.001 * 0.001 / 2.0;

        // Act
        var result = _sut.CalculateArea(diagonal1, diagonal2);

        // Assert
        result.Should().BeApproximately(expected, 1e-10);
    }

    [Fact]
    public void CalculatePerimeter_WithLargeSide_ReturnsCorrectPerimeter()
    {
        // Arrange
        var side = 1_000_000.0;

        // Act
        var result = _sut.CalculatePerimeter(side);

        // Assert
        result.Should().BeApproximately(4_000_000.0, 0.0001);
    }
}
