using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Interfaces.Sorting;

namespace CopilotMultiAgent.Application.Sorting;

public class BubbleSortAlgorithm : ISortingAlgorithm
{
    public IEnumerable<int> Sort(IEnumerable<int> numbers, SortDirection direction)
    {
        var arr = numbers.ToArray();
        var compare = SortHelper.GetComparison(direction);
        for (int i = 0; i < arr.Length - 1; i++)
            for (int j = 0; j < arr.Length - 1 - i; j++)
                if (compare(arr[j], arr[j + 1]) > 0)
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
        return arr;
    }
}
