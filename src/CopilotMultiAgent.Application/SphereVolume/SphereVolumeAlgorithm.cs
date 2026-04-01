using CopilotMultiAgent.Domain.Interfaces.SphereVolume;

namespace CopilotMultiAgent.Application.SphereVolume;

public class SphereVolumeAlgorithm : ISphereVolumeAlgorithm
{
    public double Calculate(double radius)
    {
        if (radius <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radius), radius, "Radius must be greater than zero.");
        }

        return (4.0 / 3.0) * Math.PI * Math.Pow(radius, 3);
    }
}
