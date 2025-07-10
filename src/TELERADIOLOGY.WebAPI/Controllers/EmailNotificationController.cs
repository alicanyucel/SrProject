using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.API.Controllers
{
    [AllowAnonymous]
    public class EmailController : ApiController
    {
        public EmailController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            var command = new SendEmailCommand(request.To, request.Subject, request.Message);
            var result = await _mediator.Send(command);

            if (result.IsSuccessful)
            {
                return Ok(new
                {
                    success = true,
                    message = "E-posta başarıyla gönderildi.",
                    data = result.Data
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "E-posta gönderimi başarısız.",
                errors = result.ErrorMessages
            });
        }
    }
}