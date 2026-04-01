using System.ComponentModel.DataAnnotations;
using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.DTOs;

public record CreateTodoDto(
    [Required][MaxLength(200)] string Title,
    string? Description = null,
    DateTime? DueDate = null,
    Priority Priority = Priority.Low,
    IEnumerable<string>? Tags = null);
