using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Domain.Interfaces.Sorting;

public interface ISortingAlgorithm
{
    IEnumerable<int> Sort(IEnumerable<int> numbers, SortDirection direction);
}
