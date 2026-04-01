using CopilotMultiAgent.Domain.Interfaces.Fibonacci;

namespace CopilotMultiAgent.Application.Fibonacci;

public class IterativeFibonacciAlgorithm : IFibonacciAlgorithm
{
    public IReadOnlyList<long> Generate(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
        }

        var sequence = new long[count];
        sequence[0] = 0;

        if (count == 1)
        {
            return sequence;
        }

        sequence[1] = 1;

        for (int i = 2; i < count; i++)
        {
            sequence[i] = sequence[i - 1] + sequence[i - 2];
        }

        return sequence;
    }
}
