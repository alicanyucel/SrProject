using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Permissions.CreatePermission;
using TELERADIOLOGY.Application.Features.Permissions.DeletePermission;
using TELERADIOLOGY.Application.Features.Permissions.GetAllPermission;
using TELERADIOLOGY.Application.Features.Permissions.GetByIdPermission;
using TELERADIOLOGY.Application.Features.Permissions.UpdatePermission;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class PermissionController : ApiController
{
    private readonly ILogger<PermissionController> _logger;

    public PermissionController(IMediator mediator, ILogger<PermissionController> logger) : base(mediator)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePermissionCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccessful)
                return Ok(result);
            _logger.LogWarning("Kullanıcı eklenemedi: {Message}", result.ErrorMessages);
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ekleme hatası.");
            return StatusCode(500, "Server hatası.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetAllPermissionsQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetAll hatası.");
            return StatusCode(500, "Server hatası.");
        }
    }

    [HttpPost("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetPermissionByIdQuery(id));
            if (result.IsSuccessful)
                return Ok(result);
            return NotFound(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetById hatası");
            return StatusCode(500, "Server hatası");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionCommand command)
    {
        if (id != command.Id)
            return BadRequest("id bulunamadı");

        try
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccessful)
                return Ok(result);
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Güncellenemedi");
            return StatusCode(500, "Server hatası.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _mediator.Send(new DeletePermissionByIdCommand(id));
            if (result.IsSuccessful)
                return Ok(result);
            return NotFound(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Silinemedi");
            return StatusCode(500, "Server hatası.");
        }
    }
}
