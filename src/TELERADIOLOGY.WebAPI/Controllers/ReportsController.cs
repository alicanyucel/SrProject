using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Reports.CreateReport;
using TELERADIOLOGY.Application.Features.Reports.DeleteReport;
using TELERADIOLOGY.Application.Features.Reports.GetAllReport;
using TELERADIOLOGY.Application.Features.Reports.GetReportById;
using TELERADIOLOGY.Application.Features.Reports.UpdateReport;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class ReportsController : ApiController
{
    public ReportsController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteReportById(DeleteReportByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);

    }
    [HttpPost]
    public async Task<IActionResult> GetAllReports([FromQuery] GetAllReportQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);

    }
    [HttpPost("{id}")]
    public async Task<IActionResult> GetReportById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return BadRequest("Geçersiz GUID");

        var report = await _mediator.Send(new GetReportByIdQuery(guidId));
        return report == null ? NotFound() : Ok(report);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateReport(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}

