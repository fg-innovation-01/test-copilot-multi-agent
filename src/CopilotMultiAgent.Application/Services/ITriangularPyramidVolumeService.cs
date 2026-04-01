using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface ITriangularPyramidVolumeService
{
    TriangularPyramidVolumeResultDto Calculate(TriangularPyramidVolumeRequestDto request);
}
