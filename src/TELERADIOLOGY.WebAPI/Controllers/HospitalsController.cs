using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Hospitals.CreateHospital;
using TELERADIOLOGY.Application.Features.Hospitals.DeleteHospital;
using TELERADIOLOGY.Application.Features.Hospitals.GetAllHospital;
using TELERADIOLOGY.Application.Features.Hospitals.GetHospitalById;
using TELERADIOLOGY.Application.Features.Hospitals.HospitalFilter;
using TELERADIOLOGY.Application.Features.Hospitals.UpdateHospital;
using TELERADIOLOGY.WebAPI.Abstractions;

[AllowAnonymous]
public class HospitalsController : ApiController
{
    public HospitalsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> GetAllHospitals(GetAllHospitalQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHospital(CreateHospitalCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateHospital(UpdateHospitalCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteHospitalById(DeleteHospitalByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> GetHospitalById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return BadRequest("Geçersiz GUID");

        var hospial = await _mediator.Send(new GetHospitalByIdQuery(guidId));
        return hospial == null ? NotFound() : Ok(hospial);
    }

    [HttpPost]
    public async Task<IActionResult> HospitalFilter([FromBody] GetHospitalsByFilterQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        if (response.IsSuccessful)
            return Ok(response);

        return BadRequest(response.ErrorMessages);
    }

}