using CopilotMultiAgent.Domain.Interfaces.CubeVolume;

namespace CopilotMultiAgent.Application.CubeVolume;

public class CubeVolumeAlgorithm : ICubeVolumeAlgorithm
{
    public double Calculate(double sideLength)
    {
        if (sideLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sideLength), sideLength, "Side length must be greater than zero.");
        }

        return Math.Pow(sideLength, 3);
    }
}
