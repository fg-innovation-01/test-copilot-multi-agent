namespace CopilotMultiAgent.Application.DTOs;

public record FibonacciResultDto(IReadOnlyList<long> Sequence, long ElapsedMs);
