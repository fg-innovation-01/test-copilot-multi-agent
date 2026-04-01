using CopilotMultiAgent.Domain.Enums;

namespace CopilotMultiAgent.Application.Sorting;

internal static class SortHelper
{
    internal static Comparison<int> GetComparison(SortDirection direction) =>
        direction == SortDirection.Ascending
            ? (a, b) => a.CompareTo(b)
            : (a, b) => b.CompareTo(a);
}
