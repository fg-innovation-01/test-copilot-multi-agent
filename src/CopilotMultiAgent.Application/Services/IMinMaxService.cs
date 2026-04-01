using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface IMinMaxService
{
    MinMaxResultDto Calculate(MinMaxRequestDto request);
}
