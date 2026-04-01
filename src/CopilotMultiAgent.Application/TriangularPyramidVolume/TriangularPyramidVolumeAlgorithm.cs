using CopilotMultiAgent.Domain.Interfaces.TriangularPyramidVolume;

namespace CopilotMultiAgent.Application.TriangularPyramidVolume;

public class TriangularPyramidVolumeAlgorithm : ITriangularPyramidVolumeAlgorithm
{
    public double Calculate(double baseEdge, double height)
    {
        if (baseEdge <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseEdge), baseEdge, "Base edge must be greater than zero.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), height, "Height must be greater than zero.");
        }

        return Math.Sqrt(3) / 12.0 * Math.Pow(baseEdge, 2) * height;
    }
}
