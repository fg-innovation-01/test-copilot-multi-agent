using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TriangularPyramidVolumeController : ControllerBase
{
    private readonly ITriangularPyramidVolumeService _service;

    public TriangularPyramidVolumeController(ITriangularPyramidVolumeService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<TriangularPyramidVolumeResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] TriangularPyramidVolumeRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}
