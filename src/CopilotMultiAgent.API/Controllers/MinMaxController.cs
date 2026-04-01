using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MinMaxController : ControllerBase
{
    private readonly IMinMaxService _service;

    public MinMaxController(IMinMaxService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<MinMaxResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] MinMaxRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}
