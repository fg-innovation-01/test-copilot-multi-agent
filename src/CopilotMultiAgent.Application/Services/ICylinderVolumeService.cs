using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface ICylinderVolumeService
{
    CylinderVolumeResultDto Calculate(CylinderVolumeRequestDto request);
}