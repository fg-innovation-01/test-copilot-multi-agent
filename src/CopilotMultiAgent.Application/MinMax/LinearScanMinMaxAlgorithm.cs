using CopilotMultiAgent.Domain.Interfaces.MinMax;

namespace CopilotMultiAgent.Application.MinMax;

public class LinearScanMinMaxAlgorithm : IMinMaxAlgorithm
{
    public (int Min, int Max) Calculate(IEnumerable<int> numbers)
    {
        using var enumerator = numbers.GetEnumerator();
        if (!enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains no elements");

        int min = enumerator.Current;
        int max = enumerator.Current;

        while (enumerator.MoveNext())
        {
            int current = enumerator.Current;
            if (current < min) min = current;
            if (current > max) max = current;
        }

        return (min, max);
    }
}
