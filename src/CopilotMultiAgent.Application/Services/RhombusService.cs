using System.Diagnostics;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Rhombus;

namespace CopilotMultiAgent.Application.Services;

public class RhombusService : IRhombusService
{
    public RhombusResultDto Calculate(RhombusRequestDto request)
    {
        var algorithm = new RhombusAlgorithm();
        var sw = Stopwatch.StartNew();
        var area = algorithm.CalculateArea(request.Diagonal1, request.Diagonal2);
        var perimeter = algorithm.CalculatePerimeter(request.Side);
        sw.Stop();
        return new RhombusResultDto(area, perimeter, sw.ElapsedMilliseconds);
    }
}
