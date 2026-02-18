using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Features.Labels.Commands.CreateLabel;
using TaskFlow.Application.Features.Labels.Commands.DeleteLabel;

namespace TaskFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LabelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LabelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateLabelCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);
            
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteLabelCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return NoContent();
    }
}
