using System.ComponentModel.DataAnnotations;
using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.DTOs;

public record SortRequestDto(
    [Required][MinLength(1)] IReadOnlyList<int> Numbers,
    SortAlgorithm Algorithm,
    SortDirection Direction);
