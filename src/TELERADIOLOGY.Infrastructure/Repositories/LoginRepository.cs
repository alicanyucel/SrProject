using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

internal class LoginRepository : Repository<Login, PostgreSqlDbContext>, ILoginRepository
{
    public LoginRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
