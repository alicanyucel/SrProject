using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories
{
    internal sealed class UserRoleRepository : Repository<AppUserRole, PostgreSqlDbContext>, IUserRoleRepository
    {
        public UserRoleRepository(PostgreSqlDbContext context) : base(context)
        {
        }
    }
}