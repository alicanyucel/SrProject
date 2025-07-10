using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class MemberRepository : Repository<Member, PostgreSqlDbContext>, IMemberRepository
{
    public MemberRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
