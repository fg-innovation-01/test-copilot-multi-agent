using CopilotMultiAgent.Application.MinMax;
using CopilotMultiAgent.Domain.Exceptions;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.MinMax;

public class LinearScanMinMaxAlgorithmTests
{
    private readonly LinearScanMinMaxAlgorithm _sut = new();

    [Fact]
    public void Calculate_WithMixedNumbers_ShouldReturnCorrectMinAndMax()
    {
        // Arrange
        var numbers = new[] { 3, -1, 7, 0, 5 };

        // Act
        var result = _sut.Calculate(numbers);

        // Assert
        result.Min.Should().Be(-1);
        result.Max.Should().Be(7);
    }

    [Fact]
    public void Calculate_WithAllSameNumbers_MinAndMaxShouldBeEqual()
    {
        // Arrange
        var numbers = new[] { 4, 4, 4 };

        // Act
        var result = _sut.Calculate(numbers);

        // Assert
        result.Min.Should().Be(4);
        result.Max.Should().Be(4);
    }

    [Fact]
    public void Calculate_SingleElement_MinAndMaxAreTheSameElement()
    {
        // Arrange
        var numbers = new[] { 42 };

        // Act
        var result = _sut.Calculate(numbers);

        // Assert
        result.Min.Should().Be(42);
        result.Max.Should().Be(42);
    }

    [Fact]
    public void Calculate_WithNegativeNumbers_ShouldHandleCorrectly()
    {
        // Arrange
        var numbers = new[] { -5, -3, -10, -1 };

        // Act
        var result = _sut.Calculate(numbers);

        // Assert
        result.Min.Should().Be(-10);
        result.Max.Should().Be(-1);
    }

    [Fact]
    public void Calculate_WithAlreadyAscendingOrder_ShouldReturnCorrectMinAndMax()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = _sut.Calculate(numbers);

        // Assert
        result.Min.Should().Be(1);
        result.Max.Should().Be(5);
    }

    [Fact]
    public void Calculate_WithAlreadyDescendingOrder_ShouldReturnCorrectMinAndMax()
    {
        // Arrange
        var numbers = new[] { 5, 4, 3, 2, 1 };

        // Act
        var result = _sut.Calculate(numbers);

        // Assert
        result.Min.Should().Be(1);
        result.Max.Should().Be(5);
    }

    [Fact]
    public void Calculate_EmptyList_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var act = () => _sut.Calculate(numbers);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Calculate_ShouldNotModifyOriginalInput()
    {
        // Arrange
        var numbers = new[] { 3, -1, 7, 0, 5 };
        var copy = numbers.ToArray();

        // Act
        try { _sut.Calculate(numbers); } catch { }

        // Assert
        numbers.Should().Equal(copy);
    }
}
