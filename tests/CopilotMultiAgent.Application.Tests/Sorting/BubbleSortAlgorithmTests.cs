using CopilotMultiAgent.Application.Sorting;
using CopilotMultiAgent.Domain.Enums;
using FluentAssertions;

namespace CopilotMultiAgent.Application.Tests.Sorting;

public class BubbleSortAlgorithmTests
{
    private readonly BubbleSortAlgorithm _sut = new();

    [Fact]
    public void Sort_Ascending_ShouldReturnNumbersInAscendingOrder()
    {
        // Arrange
        var numbers = new[] { 5, 3, 1, 4, 2 };

        // Act
        var result = _sut.Sort(numbers, SortDirection.Ascending);

        // Assert
        result.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Sort_Descending_ShouldReturnNumbersInDescendingOrder()
    {
        // Arrange
        var numbers = new[] { 5, 3, 1, 4, 2 };

        // Act
        var result = _sut.Sort(numbers, SortDirection.Descending);

        // Assert
        result.Should().Equal([5, 4, 3, 2, 1]);
    }

    [Fact]
    public void Sort_EmptyList_ShouldReturnEmpty()
    {
        // Act
        var result = _sut.Sort([], SortDirection.Ascending);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Sort_SingleElement_ShouldReturnSameElement()
    {
        // Act
        var result = _sut.Sort([42], SortDirection.Ascending);

        // Assert
        result.Should().Equal([42]);
    }

    [Fact]
    public void Sort_WithDuplicates_Ascending_ShouldReturnSortedWithDuplicates()
    {
        // Arrange
        var numbers = new[] { 3, 1, 2, 1, 3 };

        // Act
        var result = _sut.Sort(numbers, SortDirection.Ascending);

        // Assert
        result.Should().Equal([1, 1, 2, 3, 3]);
    }

    [Fact]
    public void Sort_WithNegativeNumbers_Ascending_ShouldSortCorrectly()
    {
        // Arrange
        var numbers = new[] { -3, -1, -5, 0, 2 };

        // Act
        var result = _sut.Sort(numbers, SortDirection.Ascending);

        // Assert
        result.Should().Equal([-5, -3, -1, 0, 2]);
    }

    [Fact]
    public void Sort_AlreadySorted_Ascending_ShouldReturnSameOrder()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = _sut.Sort(numbers, SortDirection.Ascending);

        // Assert
        result.Should().Equal([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Sort_ReverseSorted_Descending_ShouldReturnSameOrder()
    {
        // Arrange
        var numbers = new[] { 5, 4, 3, 2, 1 };

        // Act
        var result = _sut.Sort(numbers, SortDirection.Descending);

        // Assert
        result.Should().Equal([5, 4, 3, 2, 1]);
    }

    [Fact]
    public void Sort_ShouldNotModifyOriginalInput()
    {
        // Arrange
        var numbers = new[] { 3, 1, 2 };
        var original = numbers.ToArray();

        // Act
        _sut.Sort(numbers, SortDirection.Ascending);

        // Assert
        numbers.Should().Equal(original);
    }
}
