using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record TriangularPyramidVolumeRequestDto(
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Base edge must be greater than zero.")]
    double BaseEdge,
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Height must be greater than zero.")]
    double Height);
