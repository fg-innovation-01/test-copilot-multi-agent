using System.Diagnostics;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.SphereVolume;

namespace CopilotMultiAgent.Application.Services;

public class SphereVolumeService : ISphereVolumeService
{
    public SphereVolumeResultDto Calculate(SphereVolumeRequestDto request)
    {
        var algorithm = new SphereVolumeAlgorithm();
        var sw = Stopwatch.StartNew();
        var volume = algorithm.Calculate(request.Radius);
        sw.Stop();
        return new SphereVolumeResultDto(volume, sw.ElapsedMilliseconds);
    }
}
