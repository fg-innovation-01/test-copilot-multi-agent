using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record CylinderVolumeRequestDto(
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Radius must be greater than zero.")]
    double Radius,
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Height must be greater than zero.")]
    double Height);