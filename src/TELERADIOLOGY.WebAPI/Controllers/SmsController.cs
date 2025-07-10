using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Sms;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class SmsController : ApiController
{
    public SmsController(IMediator mediator) : base(mediator)
    {
    }
    // entegre ettim twiilodan kayıtlı user hesabı acacaz okay...
    [HttpPost]
    public async Task<IActionResult> Send([FromBody] SendSmsCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccessful)
            return Ok(result);

        return BadRequest(result);
    }
}
