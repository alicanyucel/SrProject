using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TELERADIOLOGY.Application.Features.Users.Createuser;
using TELERADIOLOGY.Application.Features.Users.DeleteUser;
using TELERADIOLOGY.Application.Features.Users.FilterUser;
using TELERADIOLOGY.Application.Features.Users.GetAllUser;
using TELERADIOLOGY.Application.Features.Users.GetUserById;
using TELERADIOLOGY.Application.Features.Users.UpdateUser;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers;

[AllowAnonymous]
public class UsersController : ApiController
{
    private readonly Serilog.ILogger _logger = Log.ForContext<UsersController>();

    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("CreateUserCommand başlatıldı: {@Request}", request);
            var response = await _mediator.Send(request, cancellationToken);
            _logger.Information("CreateUserCommand sonucu: {StatusCode}", response.StatusCode);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "CreateUserCommand sırasında hata");
            return StatusCode(500, "Kullanıcı oluşturulurken sunucu hatası");
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUserById(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("DeleteUserByIdCommand başlatıldı: {@Request}", request);
            var response = await _mediator.Send(request, cancellationToken);
            _logger.Information("DeleteUserByIdCommand sonucu: {StatusCode}", response.StatusCode);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "DeleteUserByIdCommand sırasında hata");
            return StatusCode(500, "Kullanıcı silinirken sunucu hatası");
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetAllUser([FromQuery] GetAllUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("GetAllUserQuery başlatıldı");
            var response = await _mediator.Send(request, cancellationToken);
            _logger.Information("GetAllUserQuery tamamlandı, {Count} kullanıcı bulundu", response?.Count ?? 0);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "GetAllUserQuery sırasında hata");
            return StatusCode(500, "Kullanıcılar listelenirken sunucu hatası");
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("GetUserByIdQuery başlatıldı: {Id}", id);
            if (!Guid.TryParse(id, out var guidId))
            {
                _logger.Warning("Geçersiz GUID: {Id}", id);
                return BadRequest("Geçersiz GUID");
            }

            var user = await _mediator.Send(new GetUserByIdQuery(guidId), cancellationToken);
            if (user is null)
            {
                _logger.Warning("Kullanıcı bulunamadı: {Id}", id);
                return NotFound();
            }

            _logger.Information("GetUserByIdQuery başarılı: {Id}", id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "GetUserByIdQuery sırasında hata: {Id}", id);
            return StatusCode(500, "Kullanıcı getirilirken sunucu hatası");
        }
    }
    [HttpPost]
    public async Task<IActionResult> FilterUser([FromQuery] string filterIdentityNumber, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("FilterUser sorgusu başladı: {Filter}", filterIdentityNumber);

            var query = new FilterUserQuery(filterIdentityNumber);

            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsSuccessful)
                return Ok(result);
            else
                return BadRequest(result.ErrorMessages);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "FilterUser sırasında hata oluştu");
            return StatusCode(500, "Kullanıcı filtrelenirken sunucu hatası oluştu");
        }
    }
    [HttpPost]
    public async Task<IActionResult> UpdateUser(UpdateUserByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information("UpdateUserByIdCommand başlatıldı: {@Request}", request);
            var response = await _mediator.Send(request, cancellationToken);
            _logger.Information("UpdateUserByIdCommand sonucu: {StatusCode}", response.StatusCode);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "UpdateUserByIdCommand sırasında hata");
            return StatusCode(500, "Kullanıcı güncellenirken sunucu hatası");
        }
    }
}
