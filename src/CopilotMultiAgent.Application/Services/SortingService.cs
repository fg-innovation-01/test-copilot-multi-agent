using System.Diagnostics;
using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Sorting;
using CopilotMultiAgent.Domain.Enums;
using CopilotMultiAgent.Domain.Interfaces.Sorting;

namespace CopilotMultiAgent.Application.Services;

public class SortingService : ISortingService
{
    public SortResultDto Sort(SortRequestDto request)
    {
        ISortingAlgorithm algorithm = request.Algorithm switch
        {
            SortAlgorithm.BubbleSort => new BubbleSortAlgorithm(),
            SortAlgorithm.MergeSort  => new MergeSortAlgorithm(),
            SortAlgorithm.QuickSort  => new QuickSortAlgorithm(),
            _ => throw new ArgumentOutOfRangeException(nameof(request.Algorithm))
        };

        var sw = Stopwatch.StartNew();
        var sorted = algorithm.Sort(request.Numbers, request.Direction).ToList();
        sw.Stop();

        return new SortResultDto(
            sorted,
            request.Algorithm.ToString(),
            request.Direction.ToString(),
            sw.ElapsedMilliseconds);
    }
}
