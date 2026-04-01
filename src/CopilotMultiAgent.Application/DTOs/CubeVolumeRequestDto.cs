using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record CubeVolumeRequestDto(
    [Required][Range(double.Epsilon, double.MaxValue, ErrorMessage = "Side length must be greater than zero.")]
    double SideLength);
