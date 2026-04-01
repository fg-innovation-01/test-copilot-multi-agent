using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SphereVolumeController : ControllerBase
{
    private readonly ISphereVolumeService _service;

    public SphereVolumeController(ISphereVolumeService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<SphereVolumeResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] SphereVolumeRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}
