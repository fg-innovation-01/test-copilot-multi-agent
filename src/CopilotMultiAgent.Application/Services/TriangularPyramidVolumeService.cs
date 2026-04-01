using System.Diagnostics;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.TriangularPyramidVolume;

namespace CopilotMultiAgent.Application.Services;

public class TriangularPyramidVolumeService : ITriangularPyramidVolumeService
{
    public TriangularPyramidVolumeResultDto Calculate(TriangularPyramidVolumeRequestDto request)
    {
        var algorithm = new TriangularPyramidVolumeAlgorithm();
        var sw = Stopwatch.StartNew();
        var volume = algorithm.Calculate(request.BaseEdge, request.Height);
        sw.Stop();
        return new TriangularPyramidVolumeResultDto(volume, sw.ElapsedMilliseconds);
    }
}
