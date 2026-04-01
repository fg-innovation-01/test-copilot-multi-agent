using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.CubeVolume;

public class CubeVolumeServiceTests
{
    private readonly ICubeVolumeService _sut = new CubeVolumeService();

    [Fact]
    public void Calculate_WithValidRequest_ReturnsCorrectVolume()
    {
        // Arrange
        var request = new CubeVolumeRequestDto(3.0);
        var expected = Math.Pow(3.0, 3);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Volume.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithValidRequest_ReturnsNonNegativeElapsedMs()
    {
        // Arrange
        var request = new CubeVolumeRequestDto(4.0);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
