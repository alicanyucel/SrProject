using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.AccountNotification.ForgotPassword;
using TELERADIOLOGY.Application.Features.AccountNotification.ResetPassword;
using TELERADIOLOGY.WebAPI.Abstractions;
using TS.Result;


[AllowAnonymous]
public class AccountController : ApiController
{
    public AccountController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        Result<string> result = await _mediator.Send(command);

        if (!result.IsSuccessful)
            return BadRequest(new { message = result.ErrorMessages?.FirstOrDefault() });
        return Ok(new { token = result.Data });
    }
    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        Result<string> result = await _mediator.Send(command);

        if (!result.IsSuccessful)
            return BadRequest(new { message = result.ErrorMessages?.FirstOrDefault() });

        return Ok(new { message = result.Data });
    }
}
