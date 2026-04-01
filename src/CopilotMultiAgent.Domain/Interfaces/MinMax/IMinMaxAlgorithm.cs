namespace CopilotMultiAgent.Domain.Interfaces.MinMax;

public interface IMinMaxAlgorithm
{
    (int Min, int Max) Calculate(IEnumerable<int> numbers);
}
