using CopilotMultiAgent.Application.DTOs;

namespace CopilotMultiAgent.Application.Services;

public interface ISortingService
{
    SortResultDto Sort(SortRequestDto request);
}
