using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Templates.CreateTemplate;
using TELERADIOLOGY.Application.Features.Templates.DeleteTemplateById;
using TELERADIOLOGY.Application.Features.Templates.GetAllTemplate;
using TELERADIOLOGY.Application.Features.Templates.GetTemplateById;
using TELERADIOLOGY.Application.Features.Templates.UpdateTemplate;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;
//
[AllowAnonymous]
public class TemplatesController : ApiController
{
    public TemplatesController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllTemplateQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> GetById(GetTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(DeleteTemplateByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}
