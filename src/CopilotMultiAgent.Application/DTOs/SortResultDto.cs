namespace CopilotMultiAgent.Application.DTOs;

public record SortResultDto(
    IReadOnlyList<int> SortedNumbers,
    string AlgorithmUsed,
    string Direction,
    long ElapsedMs);
