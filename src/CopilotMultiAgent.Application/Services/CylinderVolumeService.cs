using System.Diagnostics;
using CopilotMultiAgent.Application.CylinderVolume;
using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public class CylinderVolumeService : ICylinderVolumeService
{
    public CylinderVolumeResultDto Calculate(CylinderVolumeRequestDto request)
    {
        var algorithm = new CylinderVolumeAlgorithm();
        var sw = Stopwatch.StartNew();
        var volume = algorithm.Calculate(request.Radius, request.Height);
        sw.Stop();
        return new CylinderVolumeResultDto(volume, sw.ElapsedMilliseconds);
    }
}