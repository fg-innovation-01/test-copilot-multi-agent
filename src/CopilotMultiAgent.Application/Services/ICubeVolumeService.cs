using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface ICubeVolumeService
{
    CubeVolumeResultDto Calculate(CubeVolumeRequestDto request);
}
