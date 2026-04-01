using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record SphereVolumeRequestDto(
    [Required][Range(double.Epsilon, double.MaxValue, ErrorMessage = "Radius must be greater than zero.")]
    double Radius);
