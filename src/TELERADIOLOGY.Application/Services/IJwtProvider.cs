using TELERADIOLOGY.Application.Features.Auth.Login;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Application.Services;

public interface IJwtProvider
{//ESKİ COMMİTE DONULDU
    Task<LoginCommandResponse> CreateToken(AppUser user);
}
