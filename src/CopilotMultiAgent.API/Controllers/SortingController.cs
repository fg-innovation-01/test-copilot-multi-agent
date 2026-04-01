using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SortingController : ControllerBase
{
    private readonly ISortingService _service;

    public SortingController(ISortingService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<SortResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Sort([FromBody] SortRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _service.Sort(dto);
        return Ok(result);
    }
}
