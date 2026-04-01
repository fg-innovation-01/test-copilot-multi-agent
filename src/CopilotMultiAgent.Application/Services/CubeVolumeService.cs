using System.Diagnostics;
using CopilotMultiAgent.Application.CubeVolume;
using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public class CubeVolumeService : ICubeVolumeService
{
    public CubeVolumeResultDto Calculate(CubeVolumeRequestDto request)
    {
        var algorithm = new CubeVolumeAlgorithm();
        var sw = Stopwatch.StartNew();
        var volume = algorithm.Calculate(request.SideLength);
        sw.Stop();
        return new CubeVolumeResultDto(volume, sw.ElapsedMilliseconds);
    }
}
