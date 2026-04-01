using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.Fibonacci;

public class FibonacciServiceTests
{
    private readonly IFibonacciService _sut = new FibonacciService();

    [Fact]
    public void Calculate_WithValidRequest_ReturnsCorrectSequence()
    {
        // Arrange
        var request = new FibonacciRequestDto(5);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Sequence.Should().Equal(0, 1, 1, 2, 3);
    }

    [Fact]
    public void Calculate_WithValidRequest_ReturnsNonNegativeElapsedMs()
    {
        // Arrange
        var request = new FibonacciRequestDto(10);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
