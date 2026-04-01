using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record FibonacciRequestDto(
    [Required][Range(1, 93)] int Count);
