using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CylinderVolumeController : ControllerBase
{
    private readonly ICylinderVolumeService _service;

    public CylinderVolumeController(ICylinderVolumeService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<CylinderVolumeResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] CylinderVolumeRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}