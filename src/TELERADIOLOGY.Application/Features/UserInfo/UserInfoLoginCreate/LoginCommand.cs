using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.UserInfo.Login
{
    public sealed record  LoginCommand(
   int RoleId,
   string username,
   bool is_active,
   string password,
   DateTime created_date,
   DateTime last_login,
   string user_code
 ) : IRequest<Result<string>>;
}
