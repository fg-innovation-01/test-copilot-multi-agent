using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.TriangularPyramidVolume;

public class TriangularPyramidVolumeServiceTests
{
    private readonly TriangularPyramidVolumeService _sut = new();

    [Fact]
    public void Calculate_WithValidRequest_ReturnsCorrectVolume()
    {
        // Arrange
        var request = new TriangularPyramidVolumeRequestDto(3.0, 5.0);
        var expected = Math.Sqrt(3) / 12.0 * Math.Pow(3.0, 2) * 5.0;

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Volume.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithValidRequest_ReturnsNonNegativeElapsedMs()
    {
        // Arrange
        var request = new TriangularPyramidVolumeRequestDto(4.0, 6.0);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
