using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface IRhombusService
{
    RhombusResultDto Calculate(RhombusRequestDto request);
}
