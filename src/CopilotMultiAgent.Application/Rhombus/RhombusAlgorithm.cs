using CopilotMultiAgent.Domain.Interfaces.Rhombus;

namespace CopilotMultiAgent.Application.Rhombus;

public class RhombusAlgorithm : IRhombusAlgorithm
{
    public double CalculateArea(double diagonal1, double diagonal2)
    {
        if (diagonal1 <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(diagonal1), diagonal1, "Diagonal1 must be greater than zero.");
        }

        if (diagonal2 <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(diagonal2), diagonal2, "Diagonal2 must be greater than zero.");
        }

        return diagonal1 * diagonal2 / 2.0;
    }

    public double CalculatePerimeter(double side)
    {
        if (side <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(side), side, "Side must be greater than zero.");
        }

        return 4.0 * side;
    }
}
