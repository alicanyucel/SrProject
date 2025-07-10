using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure.Context;

internal class PermissionRepository : Repository<Permission, PostgreSqlDbContext>, IPermissionRepository
{
    public PermissionRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
