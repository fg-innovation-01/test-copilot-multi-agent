using System.Diagnostics;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.MinMax;

namespace CopilotMultiAgent.Application.Services;

public class MinMaxService : IMinMaxService
{
    public MinMaxResultDto Calculate(MinMaxRequestDto request)
    {
        var algorithm = new LinearScanMinMaxAlgorithm();
        var sw = Stopwatch.StartNew();
        var result = algorithm.Calculate(request.Numbers);
        sw.Stop();
        return new MinMaxResultDto(result.Min, result.Max, sw.ElapsedMilliseconds);
    }
}
