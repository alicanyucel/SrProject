using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;
using TELERADIOLOGY.Application.Features.DoctorSignatures.DeleteDoctorSignature;
using TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignature;
using TELERADIOLOGY.Application.Features.DoctorSignatures.GetAllDoctorSignatureById;
using TELERADIOLOGY.Application.Features.DoctorSignatures.UpdateDoctorSignature;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class SignaturesController : ApiController
{
    public SignaturesController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> CreateDoctorSignature(CreateDoctorSignatureCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteDoctorSignatureById(DeleteDoctorSignatureByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);

    }
    [HttpPost]
    public async Task<IActionResult> GetAllDoctorSignature([FromQuery] GetAllDoctorSignatureQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);

    }
    [HttpPost("{id}")]
    public async Task<IActionResult> GetDoctorSignatureById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return BadRequest("Geçersiz GUID");

        var signature = await _mediator.Send(new GetDoctorSignatureByIdQuery(guidId));
        return signature == null ? NotFound() : Ok(signature);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateDoctorSignature(UpdateDoctorSignatureByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
