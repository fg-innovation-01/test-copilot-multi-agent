using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface IFibonacciService
{
    FibonacciResultDto Calculate(FibonacciRequestDto request);
}
