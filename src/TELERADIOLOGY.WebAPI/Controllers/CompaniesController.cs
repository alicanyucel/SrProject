using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Companies.CreateCompany;
using TELERADIOLOGY.Application.Features.Companies.DeleteCompany;
using TELERADIOLOGY.Application.Features.Companies.FilterCompany;
using TELERADIOLOGY.Application.Features.Companies.GetAllCompany;
using TELERADIOLOGY.Application.Features.Companies.GetCompanyById;
using TELERADIOLOGY.Application.Features.Companies.UpdateCompany;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class CompaniesController : ApiController
{
    public CompaniesController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteCompanyById(DeleteCompanyByIdCommand companyId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(companyId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
    [HttpPost]
    public async Task<IActionResult> GetAllCompany([FromQuery] GetAllCompanyQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);

    }
    [HttpPost("{id}")]
    public async Task<IActionResult> GetCompanyById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return BadRequest("Geçersiz GUID");

        var company = await _mediator.Send(new GetCompanyByIdQuery(guidId));
        return company == null ? NotFound() : Ok(company);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateCompany(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> CompanyFilter([FromBody] GetCompaniesByFilterQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);

        if (response.IsSuccessful)
            return Ok(response); 

        return BadRequest(response.ErrorMessages);
    }
}