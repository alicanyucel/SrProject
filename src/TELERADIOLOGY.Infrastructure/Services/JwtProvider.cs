using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TELERADIOLOGY.Application.Features.Auth.Login;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure.Options;

internal class JwtProvider(
    UserManager<AppUser> userManager,
    IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    public async Task<LoginCommandResponse> CreateToken(AppUser user)
    {
        List<Claim> claims = new()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Name", user.FirstName),
            new Claim("Email", user.Email ?? ""),
            new Claim("UserName", user.UserName ?? "")
        };

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        DateTime expires = DateTime.UtcNow.AddMonths(1);
        var secretKey = Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey);
        if (secretKey.Length < 64)
        {
            throw new ArgumentException("Gizli anahtar en az 512 bit (64 byte) uzunluğunda olmalıdır.");
        }
        var securityKey = new SymmetricSecurityKey(secretKey);
        JwtSecurityToken jwtSecurityToken = new(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512));
        JwtSecurityTokenHandler handler = new();
        string token = handler.WriteToken(jwtSecurityToken);
        string refreshToken = Guid.NewGuid().ToString();
        DateTime refreshTokenExpires = expires.AddHours(1);
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = refreshTokenExpires;
        await userManager.UpdateAsync(user);
        return new LoginCommandResponse(token, refreshToken, refreshTokenExpires);
    }
}
