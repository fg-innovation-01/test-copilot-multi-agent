using CopilotMultiAgent.Domain.Interfaces.CylinderVolume;

namespace CopilotMultiAgent.Application.CylinderVolume;

public class CylinderVolumeAlgorithm : ICylinderVolumeAlgorithm
{
    public double Calculate(double radius, double height)
    {
        if (radius <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radius), radius, "Radius must be greater than zero.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), height, "Height must be greater than zero.");
        }

        return Math.PI * Math.Pow(radius, 2) * height;
    }
}