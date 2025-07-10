using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<AppUser, PostgreSqlDbContext>, IUserRepository
{
    public UserRepository(PostgreSqlDbContext context) : base(context)
    {
    }

}
