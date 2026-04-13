using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RhombusController : ControllerBase
{
    private readonly IRhombusService _service;

    public RhombusController(IRhombusService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<RhombusResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] RhombusRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}
