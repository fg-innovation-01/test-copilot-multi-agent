using System.Diagnostics;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Fibonacci;

namespace CopilotMultiAgent.Application.Services;

public class FibonacciService : IFibonacciService
{
    public FibonacciResultDto Calculate(FibonacciRequestDto request)
    {
        var algorithm = new IterativeFibonacciAlgorithm();
        var sw = Stopwatch.StartNew();
        var sequence = algorithm.Generate(request.Count);
        sw.Stop();
        return new FibonacciResultDto(sequence, sw.ElapsedMilliseconds);
    }
}
