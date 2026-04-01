using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CubeVolumeController : ControllerBase
{
    private readonly ICubeVolumeService _service;

    public CubeVolumeController(ICubeVolumeService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<CubeVolumeResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] CubeVolumeRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}
