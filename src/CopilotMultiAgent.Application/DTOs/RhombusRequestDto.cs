using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record RhombusRequestDto(
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Side must be greater than zero.")]
    double Side,
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Diagonal1 must be greater than zero.")]
    double Diagonal1,
    [Required]
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Diagonal2 must be greater than zero.")]
    double Diagonal2);
