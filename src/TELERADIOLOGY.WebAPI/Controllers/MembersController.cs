

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Members.ApproveMemberById;
using TELERADIOLOGY.Application.Features.Members.CreateMember;
using TELERADIOLOGY.Application.Features.Members.DeleteMember;
using TELERADIOLOGY.Application.Features.Members.GetAllMember;
using TELERADIOLOGY.Application.Features.Members.GetByIdMember;
using TELERADIOLOGY.Application.Features.Members.RejectMemberById;
using TELERADIOLOGY.Application.Features.Members.UpdateMember;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;
//imzalarda sorun var bakılsın......
[AllowAnonymous]
public class MembersController : ApiController
{
    public MembersController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteMemberByIdCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
    [HttpPost]
    public async Task<IActionResult> GetAllMember([FromQuery] GetAllMemberQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);

    }
    [HttpPost]
    public async Task<IActionResult> GetMemberById(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return BadRequest("Geçersiz GUID");

        var member = await _mediator.Send(new GetMemberByIdQuery(guidId));
        return member == null ? NotFound() : Ok(member);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateMember(UpdateMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> ApproveMemberById(ApproveMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost]
    public async Task<IActionResult> RejectMemberById(RejectMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);

    }

    [HttpPost]
    public async Task<IActionResult> AddInfoMemberById(AddInfoMemberByIdCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
