using CopilotMultiAgent.Application.Fibonacci;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.Fibonacci;

public class IterativeFibonacciAlgorithmTests
{
    private readonly IterativeFibonacciAlgorithm _sut = new();

    [Fact]
    public void Generate_WithCount5_ReturnsFirst5FibonacciNumbers()
    {
        // Arrange
        var count = 5;

        // Act
        var result = _sut.Generate(count);

        // Assert
        result.Should().Equal(0, 1, 1, 2, 3);
    }

    [Fact]
    public void Generate_WithCount1_ReturnsZero()
    {
        // Arrange
        var count = 1;

        // Act
        var result = _sut.Generate(count);

        // Assert
        result.Should().Equal(0);
    }

    [Fact]
    public void Generate_WithCount2_ReturnsZeroAndOne()
    {
        // Arrange
        var count = 2;

        // Act
        var result = _sut.Generate(count);

        // Assert
        result.Should().Equal(0, 1);
    }

    [Fact]
    public void Generate_WithCount10_ReturnsCorrectSequence()
    {
        // Arrange
        var count = 10;

        // Act
        var result = _sut.Generate(count);

        // Assert
        result.Should().Equal(0, 1, 1, 2, 3, 5, 8, 13, 21, 34);
    }

    [Fact]
    public void Generate_WithCountZero_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var count = 0;

        // Act
        var act = () => _sut.Generate(count);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Generate_WithNegativeCount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var count = -1;

        // Act
        var act = () => _sut.Generate(count);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Generate_WithCount93_ReturnsCorrectLastElement()
    {
        // Arrange
        var count = 93;

        // Act
        var result = _sut.Generate(count);

        // Assert
        result.Should().HaveCount(93);
        result[92].Should().Be(7540113804746346429L);
    }
}
