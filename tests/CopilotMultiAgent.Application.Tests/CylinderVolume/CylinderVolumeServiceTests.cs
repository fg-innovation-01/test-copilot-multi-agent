using CopilotMultiAgent.Application.CylinderVolume;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using FluentAssertions;
using Xunit;

namespace CopilotMultiAgent.Application.Tests.CylinderVolume;

public class CylinderVolumeServiceTests
{
    private readonly CylinderVolumeService _sut = new();

    [Fact]
    public void Calculate_WithValidRequest_ReturnsResultWithVolumeAndElapsedMs()
    {
        // Arrange
        var request = new CylinderVolumeRequestDto(3.0, 5.0);
        var expectedVolume = Math.PI * Math.Pow(3.0, 2) * 5.0;

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Should().NotBeNull();
        result.Volume.Should().BeApproximately(expectedVolume, 0.0001);
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Calculate_WithRadius1AndHeight1_ReturnsPi()
    {
        // Arrange
        var request = new CylinderVolumeRequestDto(1.0, 1.0);
        var expectedVolume = Math.PI;

        // Act
        var result = _sut.Calculate(request);

        // Assert
        result.Volume.Should().BeApproximately(expectedVolume, 0.0001);
    }
}