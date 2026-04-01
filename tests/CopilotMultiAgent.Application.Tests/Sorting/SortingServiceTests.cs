using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using CopilotMultiAgent.Domain.Enums;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.Sorting;

public class SortingServiceTests
{
    private readonly ISortingService _sut = new SortingService();

    [Fact]
    public void Sort_WithBubbleSort_Ascending_ShouldReturnSortedNumbers()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.BubbleSort, SortDirection.Ascending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.SortedNumbers.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Sort_WithMergeSort_Ascending_ShouldReturnSortedNumbers()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.MergeSort, SortDirection.Ascending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.SortedNumbers.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Sort_WithQuickSort_Ascending_ShouldReturnSortedNumbers()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.QuickSort, SortDirection.Ascending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.SortedNumbers.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Sort_Descending_ShouldReturnNumbersInDescendingOrder()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.MergeSort, SortDirection.Descending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.SortedNumbers.Should().Equal([5, 4, 3, 2, 1]);
    }

    [Theory]
    [InlineData(SortAlgorithm.BubbleSort, "BubbleSort")]
    [InlineData(SortAlgorithm.MergeSort, "MergeSort")]
    [InlineData(SortAlgorithm.QuickSort, "QuickSort")]
    public void Sort_ShouldReturnCorrectAlgorithmUsedName(SortAlgorithm algorithm, string expectedName)
    {
        // Arrange
        var request = new SortRequestDto([2, 1], algorithm, SortDirection.Ascending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.AlgorithmUsed.Should().Be(expectedName);
    }

    [Theory]
    [InlineData(SortDirection.Ascending, "Ascending")]
    [InlineData(SortDirection.Descending, "Descending")]
    public void Sort_ShouldReturnCorrectDirectionName(SortDirection direction, string expectedName)
    {
        // Arrange
        var request = new SortRequestDto([2, 1], SortAlgorithm.BubbleSort, direction);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.Direction.Should().Be(expectedName);
    }

    [Fact]
    public void Sort_ShouldMeasureElapsedMs()
    {
        // Arrange
        var request = new SortRequestDto([5, 3, 1, 4, 2], SortAlgorithm.QuickSort, SortDirection.Ascending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.ElapsedMs.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Sort_EmptyList_ShouldReturnEmptySortedNumbers()
    {
        // Arrange
        var request = new SortRequestDto([], SortAlgorithm.BubbleSort, SortDirection.Ascending);

        // Act
        var result = _sut.Sort(request);

        // Assert
        result.SortedNumbers.Should().BeEmpty();
    }
}
