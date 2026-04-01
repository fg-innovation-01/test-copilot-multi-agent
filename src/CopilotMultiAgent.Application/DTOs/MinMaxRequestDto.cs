using System.ComponentModel.DataAnnotations;

namespace CopilotMultiAgent.Application.DTOs;

public record MinMaxRequestDto(
    [Required][MinLength(1)] IReadOnlyList<int> Numbers);
