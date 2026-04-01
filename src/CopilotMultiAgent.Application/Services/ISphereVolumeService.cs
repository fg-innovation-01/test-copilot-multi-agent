using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface ISphereVolumeService
{
    SphereVolumeResultDto Calculate(SphereVolumeRequestDto request);
}
