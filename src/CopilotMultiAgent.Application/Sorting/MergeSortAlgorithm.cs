using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Interfaces.Sorting;

namespace CopilotMultiAgent.Application.Sorting;

public class MergeSortAlgorithm : ISortingAlgorithm
{
    public IEnumerable<int> Sort(IEnumerable<int> numbers, SortDirection direction)
    {
        var arr = numbers.ToArray();
        MergeSort(arr, 0, arr.Length - 1, SortHelper.GetComparison(direction));
        return arr;
    }

    private static void MergeSort(int[] arr, int left, int right, Comparison<int> compare)
    {
        if (left >= right) return;
        int mid = (left + right) / 2;
        MergeSort(arr, left, mid, compare);
        MergeSort(arr, mid + 1, right, compare);
        Merge(arr, left, mid, right, compare);
    }

    private static void Merge(int[] arr, int left, int mid, int right, Comparison<int> compare)
    {
        var temp = new int[right - left + 1];
        int i = left, j = mid + 1, k = 0;
        while (i <= mid && j <= right)
            temp[k++] = compare(arr[i], arr[j]) <= 0 ? arr[i++] : arr[j++];
        while (i <= mid) temp[k++] = arr[i++];
        while (j <= right) temp[k++] = arr[j++];
        temp.CopyTo(arr, left);
    }
}
