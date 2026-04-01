using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.SphereVolume;

public class SphereVolumeServiceTests
{
    private readonly ISphereVolumeService _sut = new SphereVolumeService();

    [Fact]
    public void Calculate_WithValidRequest_ReturnsCorrectVolume()
    {
        // Arrange
        var request = new SphereVolumeRequestDto(5.0);
        var expected = (4.0 / 3.0) * Math.PI * Math.Pow(5, 3);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Volume.Should().BeApproximately(expected, 0.0001);
    }

    [Fact]
    public void Calculate_WithValidRequest_ReturnsNonNegativeElapsedMs()
    {
        // Arrange
        var request = new SphereVolumeRequestDto(10.0);

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }
}
