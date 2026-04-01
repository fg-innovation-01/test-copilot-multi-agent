namespace CopilotMultiAgent.Domain.Interfaces.Fibonacci;

public interface IFibonacciAlgorithm
{
    IReadOnlyList<long> Generate(int count);
}
