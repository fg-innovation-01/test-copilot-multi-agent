using CopilotMultiAgent.Application.DTOs;
using CopilotMultiAgent.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CopilotMultiAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FibonacciController : ControllerBase
{
    private readonly IFibonacciService _service;

    public FibonacciController(IFibonacciService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType<FibonacciResultDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Calculate([FromBody] FibonacciRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = _service.Calculate(dto);
        return Ok(result);
    }
}
