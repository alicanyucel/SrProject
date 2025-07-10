using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.CompanyUsers.DeleteCompanyUser;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetAllCompanyUser;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUser;
using TELERADIOLOGY.Application.Features.CompanyUsers.GetCompanyUsersByCompanyId;
using TELERADIOLOGY.Application.Features.CompanyUsers.UpdateCompanyUser;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class CompanyUsersController : ApiController
{
    public CompanyUsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> GetUserByIdentityNumber(GetCompanyUsersByIdentityNumberQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> GetCompanyUsersByCompanyId(GetCompanyUsersByCompanyIdQuery request,CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllCompanyUsersQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompanyUser(CreateCompanyUserCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCompanyUser(UpdateCompanyUserCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCompanyUser(DeleteCompanyUserByUserIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}