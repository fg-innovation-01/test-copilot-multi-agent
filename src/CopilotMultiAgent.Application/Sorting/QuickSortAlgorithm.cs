using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Interfaces.Sorting;

namespace CopilotMultiAgent.Application.Sorting;

public class QuickSortAlgorithm : ISortingAlgorithm
{
    public IEnumerable<int> Sort(IEnumerable<int> numbers, SortDirection direction)
    {
        var arr = numbers.ToArray();
        QuickSort(arr, 0, arr.Length - 1, SortHelper.GetComparison(direction));
        return arr;
    }

    private static void QuickSort(int[] arr, int low, int high, Comparison<int> compare)
    {
        if (low >= high) return;
        int p = Partition(arr, low, high, compare);
        QuickSort(arr, low, p - 1, compare);
        QuickSort(arr, p + 1, high, compare);
    }

    private static int Partition(int[] arr, int low, int high, Comparison<int> compare)
    {
        int pivot = arr[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
            if (compare(arr[j], pivot) <= 0)
                (arr[++i], arr[j]) = (arr[j], arr[i]);
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }
}
